using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_loop : MonoBehaviour
{
    private float width;
    void Start()
    {
        //���� : Background object�� �ڽ��ݶ��̴� 2d�� X��
        width = transform.GetComponent<BoxCollider2D>().size.x;
    }

    void Update()
    {
        if (Gamemanager.Instance.isGameover || Gamemanager.Instance.isClear)
        {
            return;
        }

        if (transform.position.x <= -width)
        {
            Vector2 offset = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + offset;
        }
        
    }
}
