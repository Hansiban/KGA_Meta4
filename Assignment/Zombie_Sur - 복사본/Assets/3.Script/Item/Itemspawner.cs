using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Itemspawner : MonoBehaviour
{
    public GameObject[] items;//������
    public Transform playerTransform;//�÷��̾� ��ġ

    public float maxDistance = 5f;//�÷��̾���� �ִ�Ÿ�

    public float timeBetSpawnMax = 7f;//�ִ� �ð�
    public float timeBetSpawnMin = 2f;//�ּ� �ð�
    private float timeBetSpawn;//���� ���� ��

    private float lastSpawnTime; //������ ���� ����

    void Start()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            //������ ���� �ð� ����
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            Spawn();
        }
    }
    private void Spawn()
    {
        Vector3 spawnPosition = PointOnNavMash(playerTransform.position, maxDistance);
        spawnPosition += Vector3.up * 0.5f;

        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);

        Destroy(gameObject,5f);
    }
    private Vector3 PointOnNavMash(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;
        NavMeshHit hit;

        //maxDistance �ݰ� �ȿ��� randomPos�� ���� ����� ����޽� ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);
        return hit.position;
    }
}
