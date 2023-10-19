using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    /*
     ����
    ���콺�� �ι� �̻� Ŭ���ϸ� ������ �� �̻� �ȵǵ��� ������
    �ִϸ��̼� jump�� ���� -> �ִϸ����� grounded(false)
    ������ ����Ҷ���  Rigidbody�� �̿��ؼ� ����
    Addforce
     */

    public AudioClip DeadClip;

    [SerializeField]
    private float JumpForce = 700f;

    private int jumpcount = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D player_r;
    private Animator animatior;
    private AudioSource audio1;

    private void Start()
    {
        //������Ʈ �ʱ�ȭ (��ũ��Ʈ ����)
        player_r = transform.GetComponent<Rigidbody2D>();
        animatior = transform.GetComponent<Animator>();
        audio1 = transform.GetComponent<AudioSource>();

    }
    private void Update()
    {
        /*
         1. ����� �Է�(���콺 ���ʹ�ư)
         2. ���� ī��Ʈ Ȯ��(2)
         3. Player�� ������ ���̻� ������� �ʵ���
         */
        if (isDead)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && jumpcount < 2)
        {
            jumpcount++;
            //���� ������ �ӵ��� ���������� ���η� ����
            player_r.velocity = Vector2.zero;
            //Vector2.zero = new Vector2(0,0)
            //�÷��̾� ������ٵ� �������� ���� ��
            player_r.AddForce(new Vector2(0, JumpForce));
            //�����Ҹ�
            audio1.Play();
        }
        //���콺 ���� ��ư�� ����, ������ٵ��� �ӵ��� ���� ������̶��,
        else if (Input.GetMouseButtonUp(0) && player_r.velocity.y > 0)
        {
            player_r.velocity *= 0.5f;
        }
        animatior.SetBool("Grounded", isGrounded);

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        //�÷��̾��� �ݶ��̴� ����
        //�ٴڿ� ������� �����ϱ� ���� ���
        //��� �ݶ��̴��� �������, �浹 ǥ���� ������ ���� �ִٸ�
        if (col.contacts[0].normal.y > 0.7f)
        {
            //���� ������� ǥ��
            //����ī��Ʈ �ʱ�ȭ
            isGrounded = true;
            jumpcount = 0;
        }
    }
    //�÷��̾� �ݶ��̴� ����
    //��� ���� ������ ���
    //������ ���۵� ���
    private void OnCollisionExit2D(Collision2D col)
    {
        isGrounded = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //�񱳸޼��� : equls, 
        if (col.CompareTag("Dead") && !isDead)
        {
            //�״� �޼���
            Die();
        }
    }
    private void Die()
    {
        if (Gamemanager.Instance.isClear)
        {
            return;
        }
        //�÷��̾� ��� animation ���
        animatior.SetTrigger("Die");

        //����-> death�� ����
        audio1.clip = DeadClip;
        audio1.Play();
        //�÷��̾��� �ӵ��� ��� 0���� �����
        player_r.velocity = Vector2.zero;
        isDead = true;

        Gamemanager.Instance.Player_Dead();
    }
}
