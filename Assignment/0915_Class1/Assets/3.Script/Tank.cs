using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject Head;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, 0.1f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -0.1f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Head.transform.Rotate(Vector3.up * 1f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Head.transform.Rotate(Vector3.down * 1f);
        }
    }
}
