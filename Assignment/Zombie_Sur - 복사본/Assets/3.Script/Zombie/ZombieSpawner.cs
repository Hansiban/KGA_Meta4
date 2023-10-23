using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieData[] zombieDatas;
    public ZombieControll zombie;

    [SerializeField] private Transform[] spawnPoint;

    private List<ZombieControll> zombie_List = new List<ZombieControll>();

    private int Wave;
    private void Awake()
    {
        //SpawnPoint ����
        Setup_SpawnPoint();
    }
    private void Setup_SpawnPoint()
    {
        spawnPoint = new Transform[transform.childCount];
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            //GetChild(index) �ڽ� ��ü�� ������� ������ �ö� �����
            //��ũ��Ʈ�� ��ԵǸ� ���� Ÿ�̹��� ���� �� ����
            //�����Ҵ��̳� ������ �ٲ����ϴ� �͵��� �����Ҵ����� ��ũ��Ʈ�� �����ϴ°� �ξ� ������
            //Ư��, �迭�� ��� ������ �ٲٷ��� �ٽ� �����ؾ��ϱ� ������ ��ũ��Ʈ�� �����ϴ°� ������

            //�����Ϳ��ٰ� ��ԵǸ� ���� Ÿ�̹��� ���� �� ����
            //�����Ҵ��� ��쿡�� �����͸� ���� �����
            spawnPoint[i] = transform.GetChild(i).transform;
        }
    }
    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }
        //length�� ��� : ���̰� ������ ���� ��
        //count�� ��� : ���̰� ������ ���� ���� ��
        if (zombie_List.Count <= 0)
        {
            //���̺� �ø��� �޼ҵ�
            Spawn_Wave();
        }
        Update_UI();
        //UI ������Ʈ
    }
    private void Update_UI()
    {
        UIController.instance.Update_Wavetext(Wave, zombie_List.Count);
    }
    private void Spawn_Wave()
    {
        //���̺� ����
        Wave++;
        //���� ���� �� ���� ������� ����
        int count = Mathf.RoundToInt(Wave * 2f);
        for (int i = 0; i < count; i++)
        {
            CreateZombie();
        }

    }
    private void CreateZombie()
    {
        /* 
        zombie data �����ϰ� ������
        zombie spawnpoint �����ϰ� ������

        ���� ���̵Ǿ����� �̺�Ʈ �߰�
        1. List���� ����
        2. ���� ������Ʈ ����
        3. ���� ���
        */
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        Transform point = spawnPoint[Random.Range(0, spawnPoint.Length)];
        ZombieControll zombie = Instantiate(this.zombie, point.position, point.rotation);
        zombie.Setup(data);
        zombie_List.Add(zombie);
        //----------------�¾
        //�͸��Լ� ���
        //�޼ҵ� ������ �ȵż� �����ϸ� �ȵ�
        zombie.onDead += () => { zombie_List.Remove(zombie); }; 
        zombie.onDead += () => { Destroy(zombie.gameObject, 10f); };
        zombie.onDead += () => { GameManager.Instance.AddScore(10); };

    }
}
