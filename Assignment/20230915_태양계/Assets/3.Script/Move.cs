using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour

{
    public float pos_speed;
    public Transform Body;
    void Update()
    {
        //�߽����� ��� �̵�
        transform.RotateAround(Body.position, Vector3.up, pos_speed * Time.deltaTime);
    }
}
