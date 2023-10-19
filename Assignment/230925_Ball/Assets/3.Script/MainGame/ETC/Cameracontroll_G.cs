using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroll_G : MonoBehaviour
{
    private float wDelta = 0.35f;
    private float hDelta = 0.6f;

    public void Setup(int width, int hieght)
    {
        float size = (width > hieght) ? width * wDelta : hieght * hDelta;
        GetComponent<Camera>().orthographicSize = size;
    }

}
