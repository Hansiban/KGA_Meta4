using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Movespeed = 5f;
    [SerializeField] private float rotateSpeed = 180f;

    [SerializeField] private PlayerInput player_Input;

    private Rigidbody player_r;
    private Animator player_ani;

    private void Start()
    {
        TryGetComponent(out player_Input);
        TryGetComponent(out player_r);
        TryGetComponent(out player_ani);
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        player_ani.SetFloat("Move", player_Input.Move_Value);
    }

    private void Move()
    {
        Vector3 moveDirection = player_Input.Move_Value * transform.forward * Movespeed * Time.deltaTime;
        player_r.MovePosition(player_r.position + moveDirection);
    }
    private void Rotate()
    {
        float turn = player_Input.Rotate_Value * rotateSpeed * Time.deltaTime;
        player_r.rotation = player_r.rotation * Quaternion.Euler(0, turn, 0);
    }
}
