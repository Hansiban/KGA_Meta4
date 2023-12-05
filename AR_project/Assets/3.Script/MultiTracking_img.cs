using System.Collections.Generic;
using UnityEngine;
//추가 목록
using UnityEngine.XR.ARFoundation;

#region 주의사항
/*
 프리팹의 이름이 반드시 이미지의 이름과 같아야함
 */
#endregion

//ARTrackedImageManager 컴포넌트가 무조건 있어야 함
[RequireComponent(typeof(ARTrackedImageManager))]
public class MultiTracking_img : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    //다른 오브젝트를 불러오기 위해 배열로 게임 오브젝트를 받음
    [SerializeField] private GameObject[] GameObjects;

    //string은 이미지의 이름, GameObject는 나타낼 오브젝트로 활용
    [SerializeField] private Dictionary<string, GameObject> SpawnObject;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        SpawnObject = new Dictionary<string, GameObject>();

        //foreach문 : 작은 단위 in 자료구조
        foreach (GameObject g in GameObjects)
        {
            //게임 오브젝트를 다 생성 후 active 끄기 후 트래킹이 되면 위치를 이미지로 변경
            //1.게임 오브젝트를 다 생성 후 active 끄기
            GameObject newobject = Instantiate(g);
            newobject.name = g.name;

            SpawnObject.Add(newobject.name, newobject);
            newobject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //이미지를 찾았을때 불러오는 이벤트
        trackedImageManager.trackedImagesChanged += OntrackedImageChange; 
    }

    private void OnDisable()
    {
        //이미지가 사라졌을때 불러오는 이벤트
        trackedImageManager.trackedImagesChanged -= OntrackedImageChange;
    }


    //이미지를 찾았을 때 실행
    //이미지 변경될때 실행되는 이벤트
    private void OntrackedImageChange(ARTrackedImagesChangedEventArgs e) 
    {
        foreach (ARTrackedImage tr in e.added)
        {
            //added -> 이미지가 새로 읽혔을 때
            Update_spawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.updated)
        {
            //updated -> 이미지 위치가 업데이트가 됐을 때
            Update_spawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.removed)
        {
            //removed -> 이미지가 사라졌을 때
            SpawnObject[tr.referenceImage.name].SetActive(false);
        }
    }
    private void Update_spawnObject(ARTrackedImage tr)
    {
        //2.position, rotation 업데이트 후 active 키기(트래킹이 되면 위치를 이미지로 변경)
        string Reimg_name = tr.referenceImage.name;
        Vector3 position = tr.transform.position;

        GameObject prefabs = SpawnObject[Reimg_name];

        prefabs.transform.position = position;
        prefabs.transform.rotation = tr.transform.rotation;

        SpawnObject[Reimg_name].SetActive(true);

        //다른 오브젝트 끄기-> 있으면 하나씩, 없으면 같이뜸
        //foreach (GameObject g in SpawnObject.Values)
        //{
        //    if (g.name != Reimg_name)
        //    {
        //        g.SetActive(false);
        //    }
        //}
    }
}
