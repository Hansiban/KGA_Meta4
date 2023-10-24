using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private TimingManager timingManager;
    private void Start()
    {
        timingManager = FindObjectOfType<TimingManager>();
    }
    private void Update()
    {
        //입력 했을 타이밍에 판정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timingManager.Check_Timming();
        }
    }
}
