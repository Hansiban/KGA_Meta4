using System.Collections.Generic;
using UnityEngine;
//�߰� ���
using UnityEngine.XR.ARFoundation;

#region ���ǻ���
/*
 �������� �̸��� �ݵ�� �̹����� �̸��� ���ƾ���
 */
#endregion

//ARTrackedImageManager ������Ʈ�� ������ �־�� ��
[RequireComponent(typeof(ARTrackedImageManager))]
public class MultiTracking_img : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    //�ٸ� ������Ʈ�� �ҷ����� ���� �迭�� ���� ������Ʈ�� ����
    [SerializeField] private GameObject[] GameObjects;

    //string�� �̹����� �̸�, GameObject�� ��Ÿ�� ������Ʈ�� Ȱ��
    [SerializeField] private Dictionary<string, GameObject> SpawnObject;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        SpawnObject = new Dictionary<string, GameObject>();

        //foreach�� : ���� ���� in �ڷᱸ��
        foreach (GameObject g in GameObjects)
        {
            //���� ������Ʈ�� �� ���� �� active ���� �� Ʈ��ŷ�� �Ǹ� ��ġ�� �̹����� ����
            //1.���� ������Ʈ�� �� ���� �� active ����
            GameObject newobject = Instantiate(g);
            newobject.name = g.name;

            SpawnObject.Add(newobject.name, newobject);
            newobject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //�̹����� ã������ �ҷ����� �̺�Ʈ
        trackedImageManager.trackedImagesChanged += OntrackedImageChange; 
    }

    private void OnDisable()
    {
        //�̹����� ��������� �ҷ����� �̺�Ʈ
        trackedImageManager.trackedImagesChanged -= OntrackedImageChange;
    }


    //�̹����� ã���� �� ����
    //�̹��� ����ɶ� ����Ǵ� �̺�Ʈ
    private void OntrackedImageChange(ARTrackedImagesChangedEventArgs e) 
    {
        foreach (ARTrackedImage tr in e.added)
        {
            //added -> �̹����� ���� ������ ��
            Update_spawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.updated)
        {
            //updated -> �̹��� ��ġ�� ������Ʈ�� ���� ��
            Update_spawnObject(tr);
        }
        foreach (ARTrackedImage tr in e.removed)
        {
            //removed -> �̹����� ������� ��
            SpawnObject[tr.referenceImage.name].SetActive(false);
        }
    }
    private void Update_spawnObject(ARTrackedImage tr)
    {
        //2.position, rotation ������Ʈ �� active Ű��(Ʈ��ŷ�� �Ǹ� ��ġ�� �̹����� ����)
        string Reimg_name = tr.referenceImage.name;
        Vector3 position = tr.transform.position;

        GameObject prefabs = SpawnObject[Reimg_name];

        prefabs.transform.position = position;
        prefabs.transform.rotation = tr.transform.rotation;

        SpawnObject[Reimg_name].SetActive(true);

        //�ٸ� ������Ʈ ����-> ������ �ϳ���, ������ ���̶�
        //foreach (GameObject g in SpawnObject.Values)
        //{
        //    if (g.name != Reimg_name)
        //    {
        //        g.SetActive(false);
        //    }
        //}
    }
}
