using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody player_r;
    [SerializeField] private float speed = 8f;

    private void Start()
    {
        player_r = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Addforce  vs Velocity

        //Addforce ���� ��� �������� ���� �����Ͽ� �ӵ��� ������Ű��, �̵�/����/����/������ ���� �޴� �޼���
        //Velocity�� �ӵ��� ��Ÿ���ִ� �����̹Ƿ� ����/������ �����ϰ� �־��� �ӵ��� �̵�

        /*
        if (Input.GetKey(KeyCode.W))
        {
            player_r.AddForce(0, 0, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player_r.AddForce(0, 0, -speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player_r.AddForce(-speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player_r.AddForce(speed, 0, 0);
        }
        */

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 value = new Vector3(x, 0f, z) * speed;
        player_r.velocity = value;

    }
    public void Die()
    {
        gameObject.SetActive(false);
        if (GameObject.FindObjectOfType<GameManager>().TryGetComponent(out GameManager gm))
        {
            gm.EndGame();
        }
    }
}