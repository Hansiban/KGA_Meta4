using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ColliderCornor
{
    public Vector2 Topleft;
    public Vector2 Bottomleft;
    public Vector2 BottomRight;
}
public struct ColiderChecker
{
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    public void Reset()
    {
        Up = false;
        Down = false;
        Left = false;
        Right = false;
    }
}

public enum MoveType { Left =-1, Updown = 0, Right = 1 }

public class Movement2D : MonoBehaviour
{
    [Header("Raycast Collision")]
    [SerializeField] private LayerMask CollisionLayer;

    [Header("Raycast Count")]
    [SerializeField] private int Horizontal_count = 4;
    [SerializeField] private int Vertical_count = 4;

    //raycast count에 따른 일정한 간격
    private float Horizontal_Spacing;
    private float Vertical_Spacing;

    [Header("Movement")]
    [SerializeField] private float Movespeed;
    [SerializeField] private float JumpForce = 10;
    private float gravity = -20.0f;

    private Vector3 velocity;
    private readonly float SkinWidth = 0.015f;

    private Collider2D collider2d;
    private ColliderCornor colliderCorner;
    private ColiderChecker coliderChecker;

    public MoveType movetype { get; private set; }
    //---------------------------------------------------
    public ColiderChecker iscollisionChecker => coliderChecker;
    public Transform Hittransform { get; private set; }


    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        movetype = MoveType.Updown;
    }
    private void Update()
    {
        CalculateRayCastSpancing();
        Update_coliderCorner();
        coliderChecker.Reset();
        UpdateMovement();
        if (coliderChecker.Up || coliderChecker.Down)
        {
            velocity.y = 0;
        }
        //JumpTo();
    }
    //점프
    public void JumpTo(float jumpForce = 0)
    {
        if (jumpForce != 0)
        {
            velocity.y = jumpForce;
            return;
        }
        if (coliderChecker.Down)
        {
            velocity.y = this.JumpForce;
        }
    }
    //공 브레이크
    public void Moveto(float x)
    {
        //왼쪽 오른쪽 이동 상태일 때 좌우 방향키를 누른다면?
        if (x != 0 && movetype != MoveType.Updown)
        {
            movetype = MoveType.Updown;
        }
        velocity.x = x * Movespeed;
    }
    //Stright 상태 move
    public void SetupStrightMove(MoveType type, Vector3 postition)
    {
        movetype = type;
        transform.position = postition;
        velocity.y = 0;
    
    }
    //spancing(간격) 계산
    private void CalculateRayCastSpancing()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(SkinWidth * -2);

        Horizontal_Spacing = bounds.size.y / (Horizontal_count - 1);
        Vertical_Spacing = bounds.size.x / (Vertical_count - 1);
    }
    //collider corner 위치 갱신 메소드
    private void Update_coliderCorner()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(SkinWidth * -2);

        colliderCorner.Topleft = new Vector2(bounds.min.x, bounds.max.y);
        colliderCorner.Bottomleft = new Vector2(bounds.min.x, bounds.min.y);
        colliderCorner.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }
    private void UpdateMovement()
    {
        if (movetype.Equals(MoveType.Updown))
        {
            //y축
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            //x축
            velocity.x = (int)movetype * Movespeed;
        }

        Vector3 currentVelocity = velocity * Time.deltaTime;
        //좌우로 움직일 때 
        if (currentVelocity.x != 0)
        {
            //raycast 쏘는거 만들어주세용
            RayCastHorizontal(ref currentVelocity);
            
        }
        if (currentVelocity.y != 0)
        {
            //raycast 쏘는거 만들어주세용
            RayCastVertical(ref currentVelocity);
        }
        transform.position += currentVelocity;
    }
    private void RayCastHorizontal(ref Vector3 velocity)
    {
        /*
         ref란 
        내부 메소드에서 적용된 값을 변경 
         */
        //Mathf.Sign : 음수인지 양수인지 부호를 확인하는 메소드
        float dirction = Mathf.Sign(velocity.x);//이동방향 오 : 1, 왼 : -1
        float distance = Mathf.Abs(velocity.x) + SkinWidth; //광선의 길이
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;
        for (int i = 0; i < Horizontal_count; ++i)
        {
            rayPosition = (dirction == 1) ? colliderCorner.BottomRight : colliderCorner.Bottomleft;
            rayPosition += Vector2.up * (Horizontal_Spacing * i);
            hit = Physics2D.Raycast(rayPosition, Vector2.right * dirction, distance, CollisionLayer);
            if (hit)//hit이 null이 아니냐?
            {
                //x축 속력을 광선과 오브젝트 사이의 거리로 설정(거리가 0이면 속력도 0)
                velocity.x = (hit.distance * SkinWidth) * dirction;
                //다음에 발사되는 광선의 거리설정
                distance = hit.distance;
                //현재 진행방향, 부딪힌 방향 정보를 true로 변경
                coliderChecker.Left = (dirction == -1); 
                coliderChecker.Left = (dirction == 1); 
            }
            Debug.DrawRay(rayPosition, rayPosition + Vector2.right * dirction * distance, Color.blue);
        }
        

    }
    private void RayCastVertical(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float distance = Mathf.Abs(velocity.y) + SkinWidth;
        Vector2 rayposition = Vector2.zero;
        RaycastHit2D hit;
        for (int i = 0; i < Vertical_count; i++)
        {
            rayposition = (direction == 1) ? colliderCorner.Topleft : colliderCorner.Bottomleft;
            rayposition += Vector2.right * (Vertical_Spacing * i + velocity.x);
            hit = Physics2D.Raycast(rayposition, Vector2.up * direction, distance, CollisionLayer);
            if (hit)
            {
                velocity.y = (hit.distance - SkinWidth) * direction;
                distance = hit.distance;
                coliderChecker.Down = (direction == -1);
                coliderChecker.Up = (direction == 1);
                Hittransform = hit.transform;
            }
            Debug.DrawRay(rayposition, rayposition * Vector2.up * direction * distance, Color.yellow);
        }

    }
    
}
