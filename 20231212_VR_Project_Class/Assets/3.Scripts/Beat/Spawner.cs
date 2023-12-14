using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] cubes;
    [SerializeField] private Transform[] point;
    public float bpm = 128f;
    [SerializeField] private float beat 
    {
        get
        {
            return 60f / bpm;
        }
    }

    [Header("Object_pooling")]
    private int init_count = 10; 
    [SerializeField] private List<GameObject> cube_list = new List<GameObject>();
    private int index;

    public float timmer;
    private void Start()
    {
        point = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            point[i] = transform.GetChild(i);
        }
        timmer = 0f;

        //object pooling
        init_cube(cubes);
    }

    private void Update()
    {
        if (timmer > beat)
        {
            GameObject cube;
            while(true)
            {
                int tmp = Random.Range(0, cube_list.Count);
                if (!cube_list[tmp].activeSelf) 
                {
                    cube = Spawn_cube(tmp);
                    break;
                }
            }
            float y = Random.Range(0.5f, 1.5f);
            cube.TryGetComponent<CubeControll>(out CubeControll controller);
            index = controller.color.Equals(color.red) ? 0 : 1;

            cube.transform.position = new Vector3(point[index].transform.position.x, y, point[index].transform.position.z);
            cube.transform.Rotate(transform.forward, 90f * Random.Range(0, 4));
            timmer -= beat;
        }
        timmer += Time.deltaTime;
    }
    private void init_cube(GameObject[] cube_prefab) //생성하기
    {
        for (int i = 0; i < init_count; i++)
        {
            GameObject cube;
            if (i < init_count / 2)
            {
                cube = cube_setting(0);
            }
            else
            {
                cube = cube_setting(1);
            }
            cube_list.Add(cube);
            cube.SetActive(false);
        }
    }

    private GameObject cube_setting(int index)
    {
        GameObject cube = Instantiate(cubes[index]);
        cube.TryGetComponent(out CubeControll controller);
        controller.color = (color)index;
        return cube;
    }

    private GameObject Spawn_cube(int index)
    {
        GameObject cube = cube_list[index];
        cube.SetActive(true);
        return cube;
    }
}
