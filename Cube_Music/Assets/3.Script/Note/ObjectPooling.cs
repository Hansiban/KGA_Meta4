using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectInfo
{
    public GameObject Note_ob;
    public int count; //총 개수 -> 풀링 최대 갯수
    public Transform Parent_ob;
}
public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Queue<GameObject> nodeQueue = new Queue<GameObject>();
    [SerializeField] private ObjectInfo[] stage_info = null;

    private void Start()
    {
        nodeQueue = InsertQueue(stage_info[0]);
    }
    private Queue<GameObject> InsertQueue(ObjectInfo info)
    {
        //반환할 큐 생성
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < info.count; i++)
        {
            GameObject node_clone = Instantiate(info.Note_ob, transform.position, Quaternion.identity);
            node_clone.SetActive(false);
            if (info.Parent_ob != null)
            {
                node_clone.transform.SetParent(info.Parent_ob);
            }
            else
            {
                node_clone.transform.SetParent(this.transform);
            }
            queue.Enqueue(node_clone);
        }
        return queue;
    }
}
