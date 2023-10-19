using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    [SerializeField] private Player_Controller player;
    [SerializeField] private Stage_Data stage_Data;

    //HP관련 변수들
    private float MaxHP = 2f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CURRENTHP => currentHP;

    [SerializeField] private SpriteRenderer renderer;

    private bool isDie;
    private void Awake()
    {
        //아래 두개는 같은걸 불러오지만 아래 식이 20배정도 빠름
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        //GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);

    }
    private void OnEnable()
    {
        currentHP = MaxHP; // 초기화
        renderer = GetComponent<SpriteRenderer>();
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // GetComponent는 GC를 언제나 이용, 그러나 TryGetComponent는 GC를 이용하지 않음
        // But
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player); // GetComponent보다 20배 정도 빠르다.
        GameObject.FindGameObjectWithTag("EnemySpawner").TryGetComponent(out spawner);
        renderer.color = Color.white;
        isDie = false;
        /*if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
        { // 동적으로 할당할 때 사용, 예외처리
            GameObject.FindGameObjectWithTag("Player").AddComponent<PlayerController>();
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        }*/
    }
    private void Update()
    {
        if (transform.position.y < stage_Data.LimitMin.y - 2f) // onDie
        {
            OnDie();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            OnDie();
            player.TakeDamage(1);
        }
    }
    public void OnDie()
    {
        //Enemy 사망시 호출될 메소드
        //Destroy(gameObject);
        spawner.TakeInEnemy(gameObject);
        isDie = true;

    }
    public void TakeDamage(float Damage)
    {
        currentHP -= Damage;
        if (!isDie)
        {
            StopCoroutine(Hitanimation_co());
            StartCoroutine(Hitanimation_co());
        }
        if (currentHP <= 0)
        {
            OnDie();
        }
    }

    private IEnumerator Hitanimation_co()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
    }
}