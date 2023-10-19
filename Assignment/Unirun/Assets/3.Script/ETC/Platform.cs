using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] Obstacles;
    private bool isstep = false;

    private void OnEnable()
    {
        isstep = false;
        for (int i = 0; i < Obstacles.Length; i++)
        {
            //Random�� ����� ��ֹ� Ȱ��/��Ȱ��ȭ
            if (Random.Range(0, 3).Equals(0))
            {
                //object Ȱ��ȭ �޼ҵ� : SetActive
                Obstacles[i].SetActive(true);
            }
            else
            {
                Obstacles[i].SetActive(false);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (Gamemanager.Instance.isClear)
        {
            return;
        }
        //�÷��̾ �÷����� ����� �� ������ ����
        if (col.transform.CompareTag("Player") && !isstep)
        {
            isstep = true;
            Gamemanager.Instance.AddScore(1);
        }
    }
}
