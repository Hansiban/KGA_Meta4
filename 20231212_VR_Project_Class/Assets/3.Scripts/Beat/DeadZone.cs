using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    //[SerializeField]private Spawner spawner;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Cube"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
