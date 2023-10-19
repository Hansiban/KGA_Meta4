using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    /*
     bullet ����
        ȭ�� �ٱ����� ���� �Ÿ��� ������ �Ǹ� Destoryó��
            �ʿ��Ѱ�
            1. ȭ�� ������
            2. ���� �Ÿ�
        ���� ������ ���
        �·� ������ ���
        �Ʒ��� ������ ��� -> ��
        ��� ������ ���
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
            //����� ���ʹ����� ������ �ִ°� �����ߵ�
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
