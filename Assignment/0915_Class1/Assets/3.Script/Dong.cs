using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dong : MonoBehaviour
{
    public Transform Body;
    // Update is called once per frame
    void Update()
    {
        //RotateAround : 누구를 기준으로 돌기
        //Time.deltaTime : 
        transform.RotateAround(Body.position,Vector3.up,1000f*Time.deltaTime);
    }
}
