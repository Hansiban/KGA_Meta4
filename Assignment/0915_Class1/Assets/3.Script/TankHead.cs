using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHead : MonoBehaviour
{
    void Update()
    {
        //Ű���� �Է��� input�̶�� Ŭ���� ���
        //Input.GetKeyDown�� �������� �ѹ�
        //GetKeyDown Ű�� ������
        //GetKey Ű�� ������ ����
        if (Input.GetKey(KeyCode.RightArrow)) //Ű�� ������ ��
        {
            gameObject.transform.Rotate(Vector3.up * 1f);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.Rotate(Vector3.down * 1f);
        }
    }
}
