using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Range(10f,100f)]
    [SerializeField] private float Rotate_speed = 60f;

    private void Update()
    {
        transform.Rotate(0, Rotate_speed * Time.deltaTime, 0);
    }
}
