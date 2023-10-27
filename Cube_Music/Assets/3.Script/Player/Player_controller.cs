using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public static bool isCanpressKey = true;
    [Header("이동")]
    [SerializeField] private float movespeed = 3f;
    //키보드 입력에 따른 방향
    [SerializeField] private Vector3 input_Direction = new Vector3();
    //실제로 움직이는 방향
    [SerializeField] Vector3 dest_pos = new Vector3();

    [Header("회전")]
    [SerializeField] private float spinspeed = 270f;
    //키보드 입력에 따른 방향
    [SerializeField] private Vector3 input_RotDirection = new Vector3();
    //실제로 회전하는 방향
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
    Vector3 origin_pos; //초기 위치
    //-----------------------------------------------------

    [Header("효과")]
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
        //입력 했을 타이밍에 판정
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if (isCanMove && isCanRot && isCanpressKey)
            {
                AudioManager.instance.PlaySFX("Clap");
                //플레이어 움직임 목표값 계산
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
        //input에 따른 방향 계산
        input_Direction.Set(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));

        //이동 목표값 계산
        dest_pos = transform.position + new Vector3(input_Direction.x, 0, input_Direction.z);

        //회전 목표값 계산
        //좌우(Horiziontal) -> Z축
        //앞뒤(Vertical) -> X축
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
        //거리를 계산할때 distance보다 SqrMagnitude가 더 빠름
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
        //엉덩이 올리기
        while (real_cube.position.y < effectPos_Y)
        {
            real_cube.position += new Vector3(0, effect_Speed * Time.deltaTime);
            yield return null;
        }

        //엉덩이 내리기
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
            //아직 현재 안떨어졌고 큐브가 움직이고 있을 때
            //현재 오브젝트에서 밑으로 레이캐스트를 쏘았을 때 검출되는 오브젝트가 없는 경우
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
                Debug.Log("우아아악 떨어진다");
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
