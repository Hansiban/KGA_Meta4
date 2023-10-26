using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //³ª Á×¾úÀ½
            other.GetComponentInParent<Player_controller>().ResetFalling();
        }
    }
}
