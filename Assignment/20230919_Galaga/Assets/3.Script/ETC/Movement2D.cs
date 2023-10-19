using UnityEngine;

public class Movement2D : MonoBehaviour
{

    /*
    ��ü������ �����ϴ� manager ������ ������Ʈ ��ũ��Ʈ
    �ٸ� ������Ʈ�� ������ �ʿ���

    �ϳ��ϳ� ��ɵ��� �����ϴ� ��ũ��Ʈ
    ���� �ٸ� ������Ʈ�� ������ �� �ʿ䰡 ����
     */

    public float Move_Speed = 0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        transform.position += Move_Speed * Time.deltaTime * moveDirection;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}