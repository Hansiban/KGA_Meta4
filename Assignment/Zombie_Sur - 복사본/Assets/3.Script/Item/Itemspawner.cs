using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Itemspawner : MonoBehaviour
{
    public GameObject[] items;//아이템
    public Transform playerTransform;//플레이어 위치

    public float maxDistance = 5f;//플레이어와의 최대거리

    public float timeBetSpawnMax = 7f;//최대 시간
    public float timeBetSpawnMin = 2f;//최소 시간
    private float timeBetSpawn;//받을 랜덤 값

    private float lastSpawnTime; //마지막 생성 시점

    void Start()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            //마지막 생성 시간 갱신
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

        //maxDistance 반경 안에서 randomPos에 가장 가까운 내비메시 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);
        return hit.position;
    }
}
