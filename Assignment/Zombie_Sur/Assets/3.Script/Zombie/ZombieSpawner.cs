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
        //SpawnPoint 설정
        Setup_SpawnPoint();
    }
    private void Setup_SpawnPoint()
    {
        spawnPoint = new Transform[transform.childCount];
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            //GetChild(index) 자식 객체를 순서대로 가지고 올때 사용함
            //스크립트로 잡게되면 콜할 타이밍을 잡을 수 있음
            //동적할당이나 갯수가 바뀌어야하는 것들은 동정할당으로 스크립트로 제어하는게 훨씬 유용함
            //특히, 배열의 경우 내용을 바꾸려면 다시 선언해야하기 때문에 스크립트로 제어하는게 유용함

            //에디터에다가 잡게되면 콜할 타이밍을 잡을 수 없음
            //정적할당인 경우에는 에디터를 많이 사용함
            spawnPoint[i] = transform.GetChild(i).transform;
        }
    }
    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }
        //length의 경우 : 길이가 정해져 있을 때
        //count의 경우 : 길이가 정해져 있지 않을 때
        if (zombie_List.Count <= 0)
        {
            //웨이브 늘리는 메소드
            Spawn_Wave();
        }
        Update_UI();
        //UI 업데이트
    }
    private void Update_UI()
    {
        UIController.instance.Update_Wavetext(Wave, zombie_List.Count);
    }
    private void Spawn_Wave()
    {
        //웨이브 증가
        Wave++;
        //좀비 생성 및 좀비 몇마리인지 결정
        int count = Mathf.RoundToInt(Wave * 2f);
        for (int i = 0; i < count; i++)
        {
            CreateZombie();
        }

    }
    private void CreateZombie()
    {
        /* 
        zombie data 랜덤하게 정해줌
        zombie spawnpoint 랜덤하게 정해줌

        좀비가 다이되었을때 이벤트 추가
        1. List에서 삭제
        2. 좀비 오브젝트 삭제
        3. 점수 계산
        */
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        Transform point = spawnPoint[Random.Range(0, spawnPoint.Length)];
        ZombieControll zombie = Instantiate(this.zombie, point.position, point.rotation);
        zombie.Setup(data);
        zombie_List.Add(zombie);
        //----------------태어남
        //익명함수 사용
        //메소드 추적이 안돼서 남발하면 안됨
        zombie.onDead += () => { zombie_List.Remove(zombie); }; 
        zombie.onDead += () => { Destroy(zombie.gameObject, 10f); };
        zombie.onDead += () => { GameManager.Instance.AddScore(10); };

    }
}
