using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{

    public GameObject Platform_Prefabs;
    public int count = 3;

    public float timebetSpawnMax = 2.25f;
    public float timebetSpawnMin = 1.25f;
    public float timebetSpawn;

    public float yPos_Min = -3.5f;
    public float yPos_Max = 1.5f;

    public float Xpos = 20f;

    private GameObject[] Platforms;
    private int current_index = 0;

    private Vector2 PoolPosition = new Vector2(0, -25f);
    private float LastSpawnTime;

    /*
     1. �÷����� ���� Start �ݹ� �Լ����� �� �������(3��)
     2. ���� �ð��� ���� ��ġ�� �������ְ� 3���� �������鼭 ���
     */
    private void Start()
    {
        Platforms = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            // Platform[i] = new GameObject;
            //Quaternion.identity : �����̼� 0
            Platforms[i] = Instantiate(Platform_Prefabs, PoolPosition, Quaternion.identity);
        }
        LastSpawnTime = 0;
        timebetSpawn = 0;
    }

    private void Update()
    {
        if (Gamemanager.Instance.isGameover || Gamemanager.Instance.isClear)
        {
            return;
        }

        if (Time.time >= LastSpawnTime + timebetSpawn)
        {
            LastSpawnTime = Time.time;
            timebetSpawn = Random.Range(timebetSpawnMin, timebetSpawnMax);

            float ypos = Random.Range(yPos_Max, yPos_Min);
            //Platform ������Ʈ�� onEnable �ݹ��Լ��� �θ��� ���� Ȱ��ȭ/��Ȱ��ȭ
            Platforms[current_index].SetActive(false);
            Platforms[current_index].SetActive(true);

            Platforms[current_index].transform.position = new Vector2(Xpos, ypos);

            current_index++;
            if (current_index >= count)
            {
                current_index = 0;
            }
        }
    }

}
