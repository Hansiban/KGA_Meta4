using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float Z_posEnd;
    [SerializeField] ObstacleSpawn obstacleSpawn;
    private Rigidbody rigid;

    private void Awake()
    {
        TryGetComponent(out rigid);
        obstacleSpawn = GameObject.FindWithTag("Player").GetComponent<ObstacleSpawn>();
    }
    private void Update()
    {
        if (gameObject.transform.position.z <= Z_posEnd)
        {
            obstacleSpawn.InObstacle(gameObject);
        }
        //z축으로 전진
        rigid.AddForce(Vector3.back * speed);
    }
}
