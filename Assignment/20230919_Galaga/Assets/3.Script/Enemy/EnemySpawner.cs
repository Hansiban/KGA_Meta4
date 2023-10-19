using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int Enemy_count = 5;

    private Queue<GameObject> enemyQueue;
    private Vector3 poolPosition = new Vector3(0, -25f,0);
    public static EnemySpawner instance = null;

    [SerializeField] private Stage_Data stage_Data;
    [SerializeField] private GameObject Enemy_Prefabs;
    [SerializeField] private float SpawnTime;

    [SerializeField] private GameObject Enemy_HPBar;
    [SerializeField] private Transform Canvas;


    private void Awake()
    {
        enemyQueue = new Queue<GameObject>();
        for (int i = 0; i < Enemy_count; i++)
        {
            GameObject enemy = Instantiate(Enemy_Prefabs, poolPosition, Quaternion.identity);
            enemy.SetActive(false);
            enemyQueue.Enqueue(enemy);
        }
        StartCoroutine(SpawnEnemy_Co());
    }

    private IEnumerator SpawnEnemy_Co()
    {
        WaitForSeconds wfs = new WaitForSeconds(SpawnTime);
        // while���� ���������� �ݺ��� �ȴ�.
        /*
            �ٵ� new WaitForSeconds�� �ϰ� �Ǹ� ���� GC�� ���� ����� �ȴ�.
            �׷��� WaitForSeconds ĳ���̶�� ���� ����Ͽ� GC�� �������� �ʵ��� �����.
            ĳ��: ��ǻ�ÿ��� �����ð� �ɸ��� �۾��� ����� �����ؼ� �ð��� ����� �����ϴ� ���
         */

        while (true)
        {
            float positionX = Random.Range(stage_Data.LimitMin.x, stage_Data.LimitMax.x);
            Vector3 position = new Vector3(positionX, stage_Data.LimitMax.y + 1f, 0);
            TakeOutEnemy(position);

            yield return wfs;
        }
    }

    private void SpawnEnemy_HP(EnemyControl enemy)
    {
        GameObject SliderClone = Instantiate(Enemy_HPBar);

        //�θ����� ���� �޼ҵ�
        SliderClone.transform.SetParent(Canvas);
        SliderClone.transform.localScale = Vector3.one;

        SliderClone.GetComponent<EnemyHpPositionSetter>().Setup(enemy.gameObject);
        SliderClone.GetComponent<EnemyHPViewer>().Setup(enemy);
    }


    public void TakeInEnemy(GameObject enemy)
    { // ť�� �ٽ� ����ִ� �޼ҵ�
        enemy.transform.position = poolPosition;
        if (enemy.activeSelf)
        {
            enemy.SetActive(false);
        }
        enemyQueue.Enqueue(enemy);
    }

    public void TakeOutEnemy(Vector3 position)
    { // ť���� ������ �޼ҵ�
        if (enemyQueue.Count <= 0)
        {
            return;
        }
        GameObject enemy = enemyQueue.Dequeue();
        if (!enemy.activeSelf)
        {
            enemy.SetActive(true);
        }
        enemy.transform.position = position;
        SpawnEnemy_HP(enemy.GetComponent<EnemyControl>());
    }


}
