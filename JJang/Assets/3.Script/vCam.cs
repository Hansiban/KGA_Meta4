using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VCam : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    private GameObject player;

    // Start is called before the first frame update
    void start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        player = FindObjectOfType<Rigidbody>().gameObject;
        vCam.LookAt = player.transform;


    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
