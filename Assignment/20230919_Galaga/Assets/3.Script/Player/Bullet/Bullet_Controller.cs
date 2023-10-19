using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    /*
     bullet 삭제
        화면 바깥으로 일정 거리를 나가게 되면 Destory처리
            필요한것
            1. 화면 사이즈
            2. 일정 거리
        위로 나가는 경우
        좌로 나가는 경우
        아래로 나가는 경우 -> 적
        우로 나가는 경우
     */
    [SerializeField] private Stage_Data stage_Data;
    private float destroyWieght = 2.0f;
    [SerializeField] private Player_Score playerscore;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerscore);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            playerscore.Set_Score(3);
            col.GetComponent<EnemyControl>().TakeDamage(1);
            //col.GetComponent<EnemyControl>().take
            //유경아 에너미한테 데미지 주는거 만들어야돼
            //todo 0921

            Destroy(gameObject);
        }

    }

    private void LateUpdate()
    {
        if (transform.position.y < stage_Data.LimitMin.y - destroyWieght ||
            transform.position.y > stage_Data.LimitMax.y + destroyWieght ||
            transform.position.x < stage_Data.LimitMin.x - destroyWieght ||
            transform.position.x > stage_Data.LimitMax.x + destroyWieght)
        {
            Destroy(gameObject);
        }
    }
}
