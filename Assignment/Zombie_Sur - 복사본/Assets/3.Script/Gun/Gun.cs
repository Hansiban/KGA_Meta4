using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
     총알 -> LineRenderer -> RayCast
     사거리
     발사될 위치
     Gundata
     Eftect
     총의 상태 -> Enum(재장전, 탄창이 비었을때, 발사준비)
     Audio Source

    Method  
    발사 -> Fire
    재장전 -> Reload
    Effect Play
     
     */
    public enum State
    {
        Ready,//발사 준비
        Empty,//총알 빔
        Reloading//재장전
    }
    public State state { get; private set; }
    //총알이 발사될 위치
    public Transform Fire_Transform;
    //총알 Line renderer
    public LineRenderer lineRenderer;
    //총알 발사 source
    private AudioSource audioSource;
    //사거리
    private float Distance = 50f;
    //총 Data
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
        //컴포넌트 비활성화
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
        //플레이어의 현재 총 상태가 준비 상태이면서
        //마지막 발사 시간이 현재 시간보다 작을 때 발사 가능
        if (state.Equals(State.Ready) && Time.time >= LastFireTime + data.TimebetFire)
        {
            LastFireTime = Time.time;
            //발사
            Shot();
        }
    }
    public void Shot()
    {
        //총-> RayCast
        RaycastHit hit;
        Vector3 Hitposition = Vector3.zero;

        if (Physics.Raycast(Fire_Transform.position, Fire_Transform.forward, out hit, Distance))
        {
            //총알이 맞았을 경우
            //우리가 만든 인터페이스를 가지고 와서 맞은 오브젝트한테 데미지를 줘야함
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
            //Ray가 충돌되지 않았을 경우
            //탄알이 최대 사정거리까지 날라 갔을때
            Hitposition = Fire_Transform.position + Fire_Transform.forward * Distance;
        }
        //총을 쏜 이펙트 플레이
        StartCoroutine(ShotEffect(Hitposition));
        Magammo--;
        //총알업뎃
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

        //소리 빵빵
        audioSource.PlayOneShot(data.Shot_clip);

        //라인렌더러 설정
        lineRenderer.SetPosition(0, Fire_Transform.position);
        lineRenderer.SetPosition(1, Hitposition);

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;
    }
    public bool Reload()
    {
        //현재 재장전이 필요한 지 안한지 Return할 메서드

        //이미 재장전 중 이거나, 총알이 없거나, 탄창에 이미 총알이 가득한 경우(30발인 경우) => false
        if (state.Equals(State.Reloading) || ammoRemain <= 0 || Magammo >= data.MagCapacity)
        {
            return false;
        }
        //총을 갈 수 있는 조건
        StartCoroutine(Reload_co());
        return true;
    }
    private IEnumerator Reload_co()
    {
        state = State.Reloading;
        audioSource.PlayOneShot(data.Reload_clip);
        yield return new WaitForSeconds(data.ReloadTime);
        
        //재장전 후에 계산
        int ammofill = data.MagCapacity - Magammo;

        //탄창에 채워야 할 탄약이 남은 탄약보다 많다면
        //채워야 할 탄약 수를 남은 탄약 수에 맞춰 줄인다.
        if (ammoRemain < ammofill)
        {
            ammofill = ammoRemain;
        }
        //탄창을 채우고 전체 탄창의 수를 줄인다.
        Magammo += ammofill;
        ammoRemain -= ammofill;
        state = State.Ready;
        //총알업뎃
        UIController.instance.Update_Ammotext(Magammo, ammoRemain);
    }

}
