using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CarController : MonoBehaviour
{
    public GameObject indicator;
    public GameObject mycar;

    public float relocateDistance = 1.0f;

    ARRaycastManager raymanager;
    GameObject PlaceObject;

    private void Start()
    {
        raymanager = GetComponent<ARRaycastManager>();
        indicator.SetActive(false);
    }

    private void Update()
    {
        //�ٴ� ���� �� �̹���
        DetectGround();

        //���� ��ư�� ��ġ�ƴٸ� ������Ʈ ����
        if (EventSystem.current.currentSelectedGameObject)
        {
            return;
        }
        //���� �ε������Ͱ� Ȱ��ȭ �� ��Ȳ�̰�
        //ȭ���� ��ġ�Ѵٸ�
        if (indicator.activeSelf && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                if (PlaceObject == null)
                {
                    PlaceObject = Instantiate(mycar, indicator.transform.position, indicator.transform.rotation);
                }
                else
                {
                    //���� ������ ������Ʈ�� ���� �Ÿ� �̻� ���̳� ���
                    if (Vector3.Distance(PlaceObject.transform.position, indicator.transform.position) > relocateDistance)
                    {
                        PlaceObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                    }
                }
            }
        }

    }

    private void DetectGround() //�ٴ��ν�
    {
        //ȭ�� ���߾� ��ġ
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //���̸� ���� ��� ������Ʈ�� ���� �ڷᱸ��
        List<ARRaycastHit> hitinfo = new List<ARRaycastHit>();

        if (raymanager.Raycast(screenSize, hitinfo, TrackableType.Planes))
        {
            indicator.SetActive(true);

            //ǥ�� ������Ʈ�� ��ġ ȸ������ ������ ��ġ(ȭ���� ���߾�)�� ��ġ��Ų��
            //pose : AR ����ĳ��Ʈ �浹 ������ ��ġ�� ������ ��Ÿ���� �Ӽ�. Matrix4x4 ���·� ����. 
            indicator.transform.position = hitinfo[0].pose.position;
            indicator.transform.rotation = hitinfo[0].pose.rotation;

            indicator.transform.position += indicator.transform.up * 0.01f;
        }
    }

}
