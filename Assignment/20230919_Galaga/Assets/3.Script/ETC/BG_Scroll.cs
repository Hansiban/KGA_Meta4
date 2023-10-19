using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Scroll : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private float a;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        a = boxCollider2D.size.y * transform.lossyScale.y;
    }
    private void Update()
    {
        if (transform.position.y <= -(a))
        {
            transform.position = new Vector3(0, (a -0.1f), 0);
        }
    }
}
