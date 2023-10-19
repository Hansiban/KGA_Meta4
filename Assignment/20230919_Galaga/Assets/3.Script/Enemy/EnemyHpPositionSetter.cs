using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpPositionSetter : MonoBehaviour
{
    [SerializeField] private Vector3 distanece = Vector3.up * 35f;

    private GameObject Target;//달아줄 Enemy
    private RectTransform UItransform; //얘는 UI라 transform이 없음

    public void Setup(GameObject target)
    {
        Target = target;
        UItransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (!Target.activeSelf)
        {
            Destroy(gameObject);
            return;
        }

        //Enemy 오브젝트는 위치가 지속적으로 갱신됨. 어케 따라가지?
        //UI -> Canvas 상속을 받고있는데 이걸 Enemy로 바꿔야되나?

        //카메라 상의 object 포지션을 포인트로 잡아서 vector3로 반환해준다.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(Target.transform.position);
        UItransform.position = screenPosition + distanece;


    }
}
