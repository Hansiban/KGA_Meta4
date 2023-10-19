using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpPositionSetter : MonoBehaviour
{
    [SerializeField] private Vector3 distanece = Vector3.up * 35f;

    private GameObject Target;//�޾��� Enemy
    private RectTransform UItransform; //��� UI�� transform�� ����

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

        //Enemy ������Ʈ�� ��ġ�� ���������� ���ŵ�. ���� ������?
        //UI -> Canvas ����� �ް��ִµ� �̰� Enemy�� �ٲ�ߵǳ�?

        //ī�޶� ���� object �������� ����Ʈ�� ��Ƽ� vector3�� ��ȯ���ش�.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(Target.transform.position);
        UItransform.position = screenPosition + distanece;


    }
}
