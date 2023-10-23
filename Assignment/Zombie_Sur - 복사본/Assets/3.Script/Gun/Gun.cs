using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
     �Ѿ� -> LineRenderer -> RayCast
     ��Ÿ�
     �߻�� ��ġ
     Gundata
     Eftect
     ���� ���� -> Enum(������, źâ�� �������, �߻��غ�)
     Audio Source

    Method  
    �߻� -> Fire
    ������ -> Reload
    Effect Play
     
     */
    public enum State
    {
        Ready,//�߻� �غ�
        Empty,//�Ѿ� ��
        Reloading//������
    }
    public State state { get; private set; }
    //�Ѿ��� �߻�� ��ġ
    public Transform Fire_Transform;
    //�Ѿ� Line renderer
    public LineRenderer lineRenderer;
    //�Ѿ� �߻� source
    private AudioSource audioSource;
    //��Ÿ�
    private float Distance = 50f;
    //�� Data
    public Gundata data;
    public ParticleSystem shot_Effect;
    public ParticleSystem shell_Effect;

    private float LastFireTime;

    public int ammoRemain = 100;
    public int Magammo;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        //������Ʈ ��Ȱ��ȭ
        lineRenderer.enabled = false;
    }
    private void OnEnable()
    {
        ammoRemain = data.StartAmmoRemain;
        Magammo = data.MagCapacity;

        state = State.Ready;

        LastFireTime = 0;
    }

    public void Fire()
    {
        //�÷��̾��� ���� �� ���°� �غ� �����̸鼭
        //������ �߻� �ð��� ���� �ð����� ���� �� �߻� ����
        if (state.Equals(State.Ready) && Time.time >= LastFireTime + data.TimebetFire)
        {
            LastFireTime = Time.time;
            //�߻�
            Shot();
        }
    }
    public void Shot()
    {
        //��-> RayCast
        RaycastHit hit;
        Vector3 Hitposition = Vector3.zero;

        if (Physics.Raycast(Fire_Transform.position, Fire_Transform.forward, out hit, Distance))
        {
            //�Ѿ��� �¾��� ���
            //�츮�� ���� �������̽��� ������ �ͼ� ���� ������Ʈ���� �������� �����
            IDamage target = hit.collider.GetComponent<IDamage>();
            if (target != null)
            {
                target.OnDamage(data.Damage, hit.point, hit.normal);
            }
            /*
             = if(hit.collider.tryGetComponent(out IDamage damage))
            {
                target. onDamage(data.Damage, hit.position, hit.normal);
            }
             */
            Hitposition = hit.point;
        }
        else
        {
            //Ray�� �浹���� �ʾ��� ���
            //ź���� �ִ� �����Ÿ����� ���� ������
            Hitposition = Fire_Transform.position + Fire_Transform.forward * Distance;
        }
        //���� �� ����Ʈ �÷���
        StartCoroutine(ShotEffect(Hitposition));
        Magammo--;
        //�Ѿ˾���
        UIController.instance.Update_Ammotext(Magammo, ammoRemain);
        if (Magammo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 Hitposition)
    {
        shot_Effect.Play();
        shell_Effect.Play();

        //�Ҹ� ����
        audioSource.PlayOneShot(data.Shot_clip);

        //���η����� ����
        lineRenderer.SetPosition(0, Fire_Transform.position);
        lineRenderer.SetPosition(1, Hitposition);

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;
    }
    public bool Reload()
    {
        //���� �������� �ʿ��� �� ������ Return�� �޼���

        //�̹� ������ �� �̰ų�, �Ѿ��� ���ų�, źâ�� �̹� �Ѿ��� ������ ���(30���� ���) => false
        if (state.Equals(State.Reloading) || ammoRemain <= 0 || Magammo >= data.MagCapacity)
        {
            return false;
        }
        //���� �� �� �ִ� ����
        StartCoroutine(Reload_co());
        return true;
    }
    private IEnumerator Reload_co()
    {
        state = State.Reloading;
        audioSource.PlayOneShot(data.Reload_clip);
        yield return new WaitForSeconds(data.ReloadTime);
        
        //������ �Ŀ� ���
        int ammofill = data.MagCapacity - Magammo;

        //źâ�� ä���� �� ź���� ���� ź�ຸ�� ���ٸ�
        //ä���� �� ź�� ���� ���� ź�� ���� ���� ���δ�.
        if (ammoRemain < ammofill)
        {
            ammofill = ammoRemain;
        }
        //źâ�� ä��� ��ü źâ�� ���� ���δ�.
        Magammo += ammofill;
        ammoRemain -= ammofill;
        state = State.Ready;
        //�Ѿ˾���
        UIController.instance.Update_Ammotext(Magammo, ammoRemain);
    }

}
