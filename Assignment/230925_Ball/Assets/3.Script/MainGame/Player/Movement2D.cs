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

    //raycast count�� ���� ������ ����
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
    //����
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
    //�� �극��ũ
    public void Moveto(float x)
    {
        //���� ������ �̵� ������ �� �¿� ����Ű�� �����ٸ�?
        if (x != 0 && movetype != MoveType.Updown)
        {
            movetype = MoveType.Updown;
        }
        velocity.x = x * Movespeed;
    }
    //Stright ���� move
    public void SetupStrightMove(MoveType type, Vector3 postition)
    {
        movetype = type;
        transform.position = postition;
        velocity.y = 0;
    
    }
    //spancing(����) ���
    private void CalculateRayCastSpancing()
    {
        Bounds bounds = collider2d.bounds;
        bounds.Expand(SkinWidth * -2);

        Horizontal_Spacing = bounds.size.y / (Horizontal_count - 1);
        Vertical_Spacing = bounds.size.x / (Vertical_count - 1);
    }
    //collider corner ��ġ ���� �޼ҵ�
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
            //y��
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            //x��
            velocity.x = (int)movetype * Movespeed;
        }

        Vector3 currentVelocity = velocity * Time.deltaTime;
        //�¿�� ������ �� 
        if (currentVelocity.x != 0)
        {
            //raycast ��°� ������ּ���
            RayCastHorizontal(ref currentVelocity);
            
        }
        if (currentVelocity.y != 0)
        {
            //raycast ��°� ������ּ���
            RayCastVertical(ref currentVelocity);
        }
        transform.position += currentVelocity;
    }
    private void RayCastHorizontal(ref Vector3 velocity)
    {
        /*
         ref�� 
        ���� �޼ҵ忡�� ����� ���� ���� 
         */
        //Mathf.Sign : �������� ������� ��ȣ�� Ȯ���ϴ� �޼ҵ�
        float dirction = Mathf.Sign(velocity.x);//�̵����� �� : 1, �� : -1
        float distance = Mathf.Abs(velocity.x) + SkinWidth; //������ ����
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;
        for (int i = 0; i < Horizontal_count; ++i)
        {
            rayPosition = (dirction == 1) ? colliderCorner.BottomRight : colliderCorner.Bottomleft;
            rayPosition += Vector2.up * (Horizontal_Spacing * i);
            hit = Physics2D.Raycast(rayPosition, Vector2.right * dirction, distance, CollisionLayer);
            if (hit)//hit�� null�� �ƴϳ�?
            {
                //x�� �ӷ��� ������ ������Ʈ ������ �Ÿ��� ����(�Ÿ��� 0�̸� �ӷµ� 0)
                velocity.x = (hit.distance * SkinWidth) * dirction;
                //������ �߻�Ǵ� ������ �Ÿ�����
                distance = hit.distance;
                //���� �������, �ε��� ���� ������ true�� ����
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
