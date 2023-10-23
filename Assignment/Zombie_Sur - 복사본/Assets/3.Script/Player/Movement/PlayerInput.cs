using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string MoveAxis_name = "Vertical";
    //[SerializeField] private string Rotate_name = "Horizontal";
    [SerializeField] private string Fire = "Fire1";
    [SerializeField] private string Reload = "Reload";
    [SerializeField] private Camera cam;

    //getAxis => ��ȯ�� float
    public float Move_Value { get; private set; }
    public float Rotate_Value { get; private set; }

    //GetButton => ��ȯ�� bool
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    private void Update()
    {
        //���ӿ����� ����� �������̵��� ����
        Move_Value = Input.GetAxis(MoveAxis_name);
        //Rotate_Value = Input.GetAxis(Rotate_name);
        isFire = Input.GetButton(Fire);
        isReload = Input.GetButton(Reload);
        Rotate();
    }

    private void Rotate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, transform.position);
        float rayLength;
        if (GroupPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointTolook = ray.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
        }
    }
}