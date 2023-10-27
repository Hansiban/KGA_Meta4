using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public static bool isCanpressKey = true;
    [Header("�̵�")]
    [SerializeField] private float movespeed = 3f;
    //Ű���� �Է¿� ���� ����
    [SerializeField] private Vector3 input_Direction = new Vector3();
    //������ �����̴� ����
    [SerializeField] Vector3 dest_pos = new Vector3();

    [Header("ȸ��")]
    [SerializeField] private float spinspeed = 270f;
    //Ű���� �Է¿� ���� ����
    [SerializeField] private Vector3 input_RotDirection = new Vector3();
    //������ ȸ���ϴ� ����
    [SerializeField] private Quaternion dest_Rot = new Quaternion();
    public Vector3 Dest_Pos
    {
        get { return dest_pos; }
    }

    [SerializeField] private Transform fake_cube;
    [SerializeField] private Transform real_cube;

    //-----------------------------------------------------
    private bool isCanMove = true;
    private bool isCanRot = true;
    private bool isFalling = false;
    Rigidbody player_r;
    Vector3 origin_pos; //�ʱ� ��ġ
    //-----------------------------------------------------

    [Header("ȿ��")]
    [SerializeField] private float effectPos_Y = 0.25f;
    [SerializeField] private float effect_Speed = 1.5f;

    private TimingManager timingManager;
    private CameraControll cameraControll;
    private void Start()
    {
        timingManager = FindObjectOfType<TimingManager>();
        cameraControll = FindObjectOfType<CameraControll>();
        player_r = GetComponentInChildren<Rigidbody>();
        origin_pos = transform.position;
    }
    private void Update()
    {
        Check_Falling();
        //�Է� ���� Ÿ�ֿ̹� ����
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (isCanMove && isCanRot && isCanpressKey)
            {
                AudioManager.instance.PlaySFX("Clap");
                //�÷��̾� ������ ��ǥ�� ���
                Calc();
                if (timingManager.Check_Timming())
                {
                    StartAction();
                }
            }
        }
    }
    private void Calc()
    {
        //input�� ���� ���� ���
        input_Direction.Set(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));

        //�̵� ��ǥ�� ���
        dest_pos = transform.position + new Vector3(input_Direction.x, 0, input_Direction.z);

        //ȸ�� ��ǥ�� ���
        //�¿�(Horiziontal) -> Z��
        //�յ�(Vertical) -> X��
        input_RotDirection = new Vector3(-input_Direction.z, 0, input_Direction.x);
        fake_cube.RotateAround(transform.position, input_RotDirection, spinspeed);
        dest_Rot = fake_cube.rotation;
    }
    private void StartAction()
    {
        StartCoroutine(Move_co());
        StartCoroutine(Spin_co());
        StartCoroutine(Effect_co());
        StartCoroutine(cameraControll.ZoomCam());
    }
    private IEnumerator Move_co()
    {
        isCanMove = false;
        //�Ÿ��� ����Ҷ� distance���� SqrMagnitude�� �� ����
        while (Vector3.SqrMagnitude(transform.position - dest_pos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest_pos, movespeed * Time.deltaTime);
            yield return null;
        }
        transform.position = dest_pos;
        isCanMove = true;
    }
    private IEnumerator Spin_co()
    {
        isCanRot = false;
        while (Quaternion.Angle(real_cube.rotation, dest_Rot) > 0.5f)
        {
            real_cube.rotation = Quaternion.RotateTowards(real_cube.rotation, dest_Rot, spinspeed * Time.deltaTime);
            yield return null;
        }
        real_cube.rotation = dest_Rot;
        isCanRot = true;
    }
    private IEnumerator Effect_co()
    {
        //������ �ø���
        while (real_cube.position.y < effectPos_Y)
        {
            real_cube.position += new Vector3(0, effect_Speed * Time.deltaTime);
            yield return null;
        }

        //������ ������
        while(real_cube.position.y > 0)
        {
            real_cube.position -= new Vector3(0, effect_Speed * Time.deltaTime);
            yield return null;
        }
        real_cube.localPosition = Vector3.zero;
    }
    private void Check_Falling()
    {
        if (!isFalling && isCanMove)
        {
            //���� ���� �ȶ������� ť�갡 �����̰� ���� ��
            //���� ������Ʈ���� ������ ����ĳ��Ʈ�� ����� �� ����Ǵ� ������Ʈ�� ���� ���
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
                Debug.Log("��ƾƾ� ��������");
            }
        }
    }
    private void Falling()
    {
        isFalling = true;
        player_r.useGravity = true;
        player_r.isKinematic = false;
    }
    public void ResetFalling()
    {
        AudioManager.instance.PlaySFX("Falling");
        isFalling = false;
        player_r.useGravity = false;
        player_r.isKinematic = true;

        transform.position = origin_pos;
        real_cube.localPosition = Vector3.zero;

    }
}
