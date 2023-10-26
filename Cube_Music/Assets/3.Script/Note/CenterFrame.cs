using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    private bool isStartMusic = false;

    /*
     ���� �������� ����
    1. ù��° ��尡 ������ �� �뷡�� �ѹ� �÷���
    -> ù��° ��尡 �浹�Ǿ����� �뷡�� �ѹ� �÷���
    -> trigger ���
     */

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isStartMusic)
        {
            if (col.CompareTag("Note"))
            {
                AudioManager.instance.PlayBGM("Stage1");
                isStartMusic = true;
            }
        }
    }
}
