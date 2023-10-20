using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ZombieControll : LivingEntity
{
    [Header("추적할 대상 레이어")]
    public LayerMask TargetLayer;
    private LivingEntity Targetentity;

    //경로를 계산할 AI Agent
    private NavMeshAgent agent;

    [Header("효과")]
    [SerializeField] private AudioClip DeathClip;
    [SerializeField] private AudioClip HitClip;
    [SerializeField] private ParticleSystem hitEffect;

    private Animator zombie_ani;
    private AudioSource zombie_audio;

    /* 나중에 컴포넌트 추가 */
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
         좀비의 입장
            플레이어한테 총알을 맞았을 때
            소리 내줘야함 & HitEffect -> 총알이 날라온 방향
         */
        if (!isDead)
        {
            hitEffect.transform.position = hitposition;
            //hit 회전값을 바라보는 회전의 상태로 변환
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
        //닿고 있을 때 지속적으로 호출
        /*
        enter - 닿기 시작
        stay - 닿고 있을 때
        exit - 닿는 것이 끝날 때
         */
        if (!isDead && Time.time >= LastAttackTimebet + TimebetAttack)
        {
            if (other.TryGetComponent(out LivingEntity e))
            {
                if (Targetentity.Equals(e))
                {
                    LastAttackTimebet = Time.time;
                    //ClosestPoint => 닿는 위치
                    //즉 상대방 피격 위치와 피격 방향을 근사값으로 계산
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
            yield return null;//1프레임씩
        }
    }
}
