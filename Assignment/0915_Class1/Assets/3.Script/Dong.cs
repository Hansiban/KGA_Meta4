using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dong : MonoBehaviour
{
    public Transform Body;
    // Update is called once per frame
    void Update()
    {
        //RotateAround : ������ �������� ����
        //Time.deltaTime : 
        transform.RotateAround(Body.position,Vector3.up,1000f*Time.deltaTime);
    }
}
