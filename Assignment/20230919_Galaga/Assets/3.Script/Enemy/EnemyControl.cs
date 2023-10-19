using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    [SerializeField] private Player_Controller player;
    [SerializeField] private Stage_Data stage_Data;

    //HP���� ������
    private float MaxHP = 2f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CURRENTHP => currentHP;

    [SerializeField] private SpriteRenderer renderer;

    private bool isDie;
    private void Awake()
    {
        //�Ʒ� �ΰ��� ������ �ҷ������� �Ʒ� ���� 20������ ����
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        //GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);

    }
    private void OnEnable()
    {
        currentHP = MaxHP; // �ʱ�ȭ
        renderer = GetComponent<SpriteRenderer>();
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // GetComponent�� GC�� ������ �̿�, �׷��� TryGetComponent�� GC�� �̿����� ����
        // But
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player); // GetComponent���� 20�� ���� ������.
        GameObject.FindGameObjectWithTag("EnemySpawner").TryGetComponent(out spawner);
        renderer.color = Color.white;
        isDie = false;
        /*if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
        { // �������� �Ҵ��� �� ���, ����ó��
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
        //Enemy ����� ȣ��� �޼ҵ�
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