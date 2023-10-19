using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //이동
        //gameObject.transform.position += new Vector3(0,0.1f,0);

        //회전
        gameObject.transform.Rotate(Vector3.up * 0.5f);
    }
}
