using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    /*
     로직
    마우스를 두번 이상 클릭하면 점프가 더 이상 안되도록 설정함
    애니메이션 jump로 변경 -> 애니메이터 grounded(false)
    점프를 사용할때는  Rigidbody를 이용해서 만듬
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
        //컴포넌트 초기화 (스크립트 기준)
        player_r = transform.GetComponent<Rigidbody2D>();
        animatior = transform.GetComponent<Animator>();
        audio1 = transform.GetComponent<AudioSource>();

    }
    private void Update()
    {
        /*
         1. 사용자 입력(마우스 왼쪽버튼)
         2. 점프 카운트 확인(2)
         3. Player가 죽으면 더이상 실행되지 않도록
         */
        if (isDead)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && jumpcount < 2)
        {
            jumpcount++;
            //점프 직전의 속도를 순간적으로 제로로 변경
            player_r.velocity = Vector2.zero;
            //Vector2.zero = new Vector2(0,0)
            //플레이어 리지드바디에 위쪽으로 힘을 줌
            player_r.AddForce(new Vector2(0, JumpForce));
            //점프소리
            audio1.Play();
        }
        //마우스 왼쪽 버튼을 떼고, 리지드바디의 속도가 위로 상승중이라면,
        else if (Input.GetMouseButtonUp(0) && player_r.velocity.y > 0)
        {
            player_r.velocity *= 0.5f;
        }
        animatior.SetBool("Grounded", isGrounded);

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        //플레이어의 콜라이더 기준
        //바닥에 닿았음을 감지하기 위해 사용
        //어떠한 콜라이더가 닿았으며, 충돌 표면이 위쪽을 보고 있다면
        if (col.contacts[0].normal.y > 0.7f)
        {
            //땅에 닿았음을 표시
            //점프카운트 초기화
            isGrounded = true;
            jumpcount = 0;
        }
    }
    //플레이어 콜라이더 기준
    //닿는 것이 끝났을 경우
    //점프가 시작될 경우
    private void OnCollisionExit2D(Collision2D col)
    {
        isGrounded = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //비교메서드 : equls, 
        if (col.CompareTag("Dead") && !isDead)
        {
            //죽는 메서드
            Die();
        }
    }
    private void Die()
    {
        if (Gamemanager.Instance.isClear)
        {
            return;
        }
        //플레이어 사망 animation 출력
        animatior.SetTrigger("Die");

        //점프-> death로 변경
        audio1.clip = DeadClip;
        audio1.Play();
        //플레이어의 속도를 모두 0으로 만들기
        player_r.velocity = Vector2.zero;
        isDead = true;

        Gamemanager.Instance.Player_Dead();
    }
}
