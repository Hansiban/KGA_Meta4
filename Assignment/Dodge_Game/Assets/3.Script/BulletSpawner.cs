using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private float SpawnRate_Min = 0.5f;
    [SerializeField] private float SpawnRate_Max = 3f;
    [SerializeField] private RaycastHit hit;
    private float Rotate_speed = 30;

    public Transform target; //player
    private float spawnRate;
    private float TimeAfterSpawn;

    private void Start()
    {
        TimeAfterSpawn = 0;
        spawnRate = Random.Range(SpawnRate_Min, SpawnRate_Max);
        target = FindObjectOfType<PlayerController>().transform;
    }
    private void Update()
    {
        DrawRay();
        transform.Rotate(0, Rotate_speed * Time.deltaTime, 0);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
    }
    private void DrawRay()
    {
        TimeAfterSpawn += Time.deltaTime;
        //����ĳ��Ʈ �׸���
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {

            if (hit.collider.gameObject.name == "Player")
            {
                if (TimeAfterSpawn >= spawnRate)
                {
                    //�Ѿ� �߻�
                    GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation);
                    TimeAfterSpawn = 0;
                }
            }
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                //�ݴ�������� ȸ��
                Rotate_speed *= -1;
                Debug.Log("Rotate_speed����");
            }
        }
        // Debug.DrawRay(transform.position, Vector3.forward * 9f, Color.red);
    }
}
