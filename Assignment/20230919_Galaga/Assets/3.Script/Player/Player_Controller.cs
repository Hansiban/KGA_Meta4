using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    //HP���� ����

    private float MaxHP = 3f;
    private float currentHP;

    [SerializeField] private Player_Score score;

    /*
     ������Ƽ �߿� Get �Լ��� �������� �ʰ� =>�� ������ �� �� ����
     */
    public float MAXHP => MaxHP;
    public float CurrentHP => currentHP;

   private SpriteRenderer renderer;

    //----
    private Movement2D movement2D;

    [SerializeField] private Stage_Data stage_Data;
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        movement2D = transform.GetComponent<Movement2D>();
        weapon = transform.GetComponent<Weapon>();
        score = GetComponent<Player_Score>();
        currentHP = MaxHP;
        TryGetComponent(out renderer);
    }

    private void Start()
    {
        if (movement2D.Move_Speed <= 0)
        {
            movement2D.Move_Speed = 5f;
        }
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement2D.MoveTo(new Vector3(x, y, 0));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weapon.StartFire();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            weapon.StopFire();
        }
    }

    private void LateUpdate()
    {
        //�÷��̾ ȭ�� ���� �ٱ����� ������ ���ϵ��� ����
        transform.position = new Vector3
        (Mathf.Clamp(transform.position.x, stage_Data.LimitMin.x, stage_Data.LimitMax.x),
        Mathf.Clamp(transform.position.y, stage_Data.LimitMin.y, stage_Data.LimitMax.y),
        0
        );
    }

    //������ and Hitaction and �״� �޼ҵ常���
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Player HP : " + currentHP);
        StopCoroutine(HitcolorAction_Co());
        StartCoroutine(HitcolorAction_Co());

        if (currentHP <= 0)
        {
            //�״¸޼���
            OnDie();
        }
    }

    private void OnDie()
    {
        score.SaveScore();
        Destroy(gameObject);
        SceneManager.LoadScene(2);
    }

    private IEnumerator HitcolorAction_Co()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
    }


}
