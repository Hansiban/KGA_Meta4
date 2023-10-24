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
     센터 프레임의 역할
    1. 첫번째 노드가 지나갈 때 노래를 한번 플레이
    -> 첫번째 노드가 충돌되었을때 노래를 한번 플레이
    -> trigger 사용
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
