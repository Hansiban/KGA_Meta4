using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    [Header("������Ʈ Ǯ��")]
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

        // ť�� count��ŭ ��ֹ� ����
        obstacleQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(obstacle, transform.position, transform.rotation);
            obj.SetActive(false);
            obstacleQueue.Enqueue(obj);
        }

        //�ڷ�ƾ ����
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

    public void InObstacle(GameObject obstacle) //�ֱ�
    {
        //������, Ȱ��ȭ �����ϱ�
        obstacle.SetActive(false);
        obstacle.transform.position = transform.position;

        //�ֱ�
        obstacleQueue.Enqueue(obstacle);
    }

    public void OutObstacle(float positionY) //����
    {
        //ť�� ��ֹ� ������ return
        if (obstacleQueue.Count <= 0)
        {
            return;
        }

        //������
        GameObject obj = obstacleQueue.Dequeue();

        //������, Ȱ��ȭ �����ϱ�
        obstacle.SetActive(true);
        obj.transform.position = startPosition + new Vector3(0, positionY, 0);
    }
}
