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
            //Random을 사용해 장애물 활성/비활성화
            if (Random.Range(0, 3).Equals(0))
            {
                //object 활성화 메소드 : SetActive
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
        //플레이어가 플랫폼을 밟았을 때 점수를 더함
        if (col.transform.CompareTag("Player") && !isstep)
        {
            isstep = true;
            Gamemanager.Instance.AddScore(1);
        }
    }
}
