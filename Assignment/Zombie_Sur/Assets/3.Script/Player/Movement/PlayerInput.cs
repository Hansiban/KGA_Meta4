using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string MoveAxis_name = "Vertical";
    [SerializeField] private string Rotate_name = "Horizontal";
    [SerializeField] private string Fire = "Fire1";
    [SerializeField] private string Reload = "Reload";

    //getAxis => 반환형 float
    public float Move_Value { get; private set; }
    public float Rotate_Value { get; private set; }
    
    //GetButton => 반환형 bool
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    private void Update()
    {
        //게임오버를 만들면 못움직이도록 선언
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            Move_Value = 0;
            Rotate_Value = 0;
            isFire = false;
            isReload = false;
            return;
        }
        Move_Value = Input.GetAxis(MoveAxis_name);
        Rotate_Value = Input.GetAxis(Rotate_name);
        isFire = Input.GetButton(Fire);
        isReload = Input.GetButton(Reload);
    }

}
