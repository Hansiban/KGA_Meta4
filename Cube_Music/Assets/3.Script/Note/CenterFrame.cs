using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    private AudioSource source;
    private bool isStartMusic = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    /*
     ���� �������� ����
    1. ù��° ��尡 ������ �� �뷡�� �ѹ� �÷���
    -> ù��° ��尡 �浹�Ǿ����� �뷡�� �ѹ� �÷���
    -> trigger ���
    2. 
     */
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isStartMusic)
        {
            if (col.CompareTag("Note"))
            {
                source.Play();
                isStartMusic = true;
            }
        }
    }
}
