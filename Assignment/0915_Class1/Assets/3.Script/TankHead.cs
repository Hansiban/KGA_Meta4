using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHead : MonoBehaviour
{
    void Update()
    {
        //키보드 입력은 input이라는 클래스 사용
        //Input.GetKeyDown은 눌렀을때 한번
        //GetKeyDown 키를 뗐을때
        //GetKey 키를 누르는 동안
        if (Input.GetKey(KeyCode.RightArrow)) //키를 눌렀을 때
        {
            gameObject.transform.Rotate(Vector3.up * 1f);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.Rotate(Vector3.down * 1f);
        }
    }
}
