using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ZombieControll : LivingEntity
{
    [Header("������ ��� ���̾�")]
    public LayerMask TargetLayer;
    private LivingEntity Targetentity;

    //��θ� ����� AI Agent
    private NavMeshAgent agent;

    [Header("ȿ��")]
    [SerializeField] private AudioClip DeathClip;
    [SerializeField] private AudioClip HitClip;
    [SerializeField] private ParticleSystem hitEffect;

    private Animator zombie_ani;
    private AudioSource zombie_audio;

    /* ���߿� ������Ʈ �߰� */
    [Header("Info")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float TimebetAttack = 0.5f;
    private float LastAttackTimebet;

    private bool isTarget
    {
        get
        {
            if (Targetentity != null && !Targetentity.isDead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        TryGetComponent(out agent);
        TryGetComponent(out zombie_ani);
        TryGetComponent(out zombie_audio);
    }

    public override void OnDamage(float Damage, Vector3 hitposition, Vector3 hitNomal)
    {
        /*
         ������ ����
            �÷��̾����� �Ѿ��� �¾��� ��
            �Ҹ� ������� & HitEffect -> �Ѿ��� ����� ����
         */
        if (!isDead)
        {
            hitEffect.transform.position = hitposition;
            //hit ȸ������ �ٶ󺸴� ȸ���� ���·� ��ȯ
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNomal);
            
            hitEffect.Play();

            zombie_audio.PlayOneShot(HitClip);
        }
        base.OnDamage(Damage, hitposition, hitNomal);
    }

    public override void Die()
    {
        base.Die();
        Collider[] colls = GetComponents<Collider>();
        foreach (Collider c in colls)
        {
            c.enabled = false;
        }
        agent.isStopped = true;
        agent.enabled = false;
        zombie_ani.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        //��� ���� �� ���������� ȣ��
        /*
        enter - ��� ����
        stay - ��� ���� ��
        exit - ��� ���� ���� ��
         */
        if (!isDead && Time.time >= LastAttackTimebet + TimebetAttack)
        {
            if (other.TryGetComponent(out LivingEntity e))
            {
                if (Targetentity.Equals(e))
                {
                    LastAttackTimebet = Time.time;
                    //ClosestPoint => ��� ��ġ
                    //�� ���� �ǰ� ��ġ�� �ǰ� ������ �ٻ簪���� ���
                    Vector3 hitpoint = other.ClosestPoint(transform.position);
                    Vector3 hitnormal = transform.position - other.transform.position;

                    e.OnDamage(damage,hitpoint,hitnormal);
                }
            }
        }
    }
    private void Update()
    {
        zombie_ani.SetBool("HasTarget", isTarget);
    }
    private void Start()
    {
        StartCoroutine(Update_Tarposition());
    }
    private IEnumerator Update_Tarposition()
    {
        while (!isDead)
        {
            if (isTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(Targetentity.transform.position);
            }
            else
            {
                agent.isStopped = true;
                Collider[] coll = Physics.OverlapSphere(transform.position, 20f, TargetLayer);
                for (int i = 0; i < coll.Length; i++)
                {
                    if (coll[i].TryGetComponent(out LivingEntity e))
                    {
                        if (!e.isDead)
                        {
                            Targetentity = e;
                            break;
                        }
                    }
                }
            }
            yield return null;//1�����Ӿ�
        }
    }
}
