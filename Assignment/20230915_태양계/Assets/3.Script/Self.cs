using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Self : MonoBehaviour
{
    public float pos_speed;
    public float rot_speed;
    public Transform Body;

    // Update is called once per frame
    void Update()
    {
        //회전
        gameObject.transform.Rotate(Vector3.up * rot_speed);
        //이동
        //transform.position += new Vector3(0, 0, pos_speed);
        //중심축을 잡고 이동
        transform.RotateAround(Body.position, Vector3.up, pos_speed * Time.deltaTime);
    }
}
