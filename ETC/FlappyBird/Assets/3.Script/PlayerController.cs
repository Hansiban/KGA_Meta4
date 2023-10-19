using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpforce;
    private Rigidbody r_player;
    private void Awake()
    {
        r_player = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //클릭 시 점프
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Input.GetMouseButtonDown(0)");
            r_player.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
    }
}
