using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //�̵�
        //gameObject.transform.position += new Vector3(0,0.1f,0);

        //ȸ��
        gameObject.transform.Rotate(Vector3.up * 0.5f);
    }
}
