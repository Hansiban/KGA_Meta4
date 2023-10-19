using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Object : MonoBehaviour
{
    [SerializeField]
    private float Speed = 10;
    // Update is called once per frame
    void Update()
    {
        if (!Gamemanager.Instance.isGameover || !Gamemanager.Instance.isClear)
        {
            transform.Translate( Speed * Time.deltaTime * Vector3.left);
        }
    }
}
