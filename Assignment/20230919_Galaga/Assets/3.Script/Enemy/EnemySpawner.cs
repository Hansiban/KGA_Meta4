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
        // while문은 지속적으로 반복이 된다.
        /*
            근데 new WaitForSeconds를 하게 되면 전부 GC의 수집 대상이 된다.
            그래서 WaitForSeconds 캐싱이라는 것을 사용하여 GC가 수집하지 않도록 만든다.
            캐싱: 컴퓨팅에서 오랜시간 걸리는 작업의 결과를 저장해서 시간과 비용을 절감하는 기법
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

        //부모설정을 위한 메소드
        SliderClone.transform.SetParent(Canvas);
        SliderClone.transform.localScale = Vector3.one;

        SliderClone.GetComponent<EnemyHpPositionSetter>().Setup(enemy.gameObject);
        SliderClone.GetComponent<EnemyHPViewer>().Setup(enemy);
    }


    public void TakeInEnemy(GameObject enemy)
    { // 큐에 다시 집어넣는 메소드
        enemy.transform.position = poolPosition;
        if (enemy.activeSelf)
        {
            enemy.SetActive(false);
        }
        enemyQueue.Enqueue(enemy);
    }

    public void TakeOutEnemy(Vector3 position)
    { // 큐에서 꺼내는 메소드
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
