using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    [Header("오브젝트 풀링")]
    [SerializeField] GameObject obstacle;
    [SerializeField] float count;
    [SerializeField] float spawntime;
    private Queue<GameObject> obstacleQueue;
    [SerializeField] Vector3 startPosition;
    [SerializeField] float maxY;
    [SerializeField] float minY;

    private void Awake()
    {
        startPosition = transform.position;
        maxY = 5;
        minY = -5;

        // 큐에 count만큼 장애물 생성
        obstacleQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(obstacle, transform.position, transform.rotation);
            obj.SetActive(false);
            obstacleQueue.Enqueue(obj);
        }

        //코루틴 시작
        SpawnEnemy_Co();
    }

    private IEnumerator SpawnEnemy_Co()
    {
        WaitForSeconds wfs = new WaitForSeconds(spawntime);
        while (true)
        {
            float positionY = Random.Range(minY, maxY);
            OutObstacle(positionY);
            yield return wfs;
        }
    }

    public void InObstacle(GameObject obstacle) //넣기
    {
        //포지션, 활성화 설정하기
        obstacle.SetActive(false);
        obstacle.transform.position = transform.position;

        //넣기
        obstacleQueue.Enqueue(obstacle);
    }

    public void OutObstacle(float positionY) //빼기
    {
        //큐에 장애물 없으면 return
        if (obstacleQueue.Count <= 0)
        {
            return;
        }

        //꺼내기
        GameObject obj = obstacleQueue.Dequeue();

        //포지션, 활성화 설정하기
        obstacle.SetActive(true);
        obj.transform.position = startPosition + new Vector3(0, positionY, 0);
    }
}
