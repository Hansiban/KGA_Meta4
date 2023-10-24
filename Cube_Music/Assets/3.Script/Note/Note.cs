using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400f;
    private Image img;
    private void OnEnable()
    {
        if (img == null)
        {
            TryGetComponent(out img);
        }
        img.enabled = true;
    }
    //노트 이미지 숨기기
    public void HideNote()
    {
        img.enabled = false;
    }
    public bool getNoteFlag()
    {
        return img.enabled;
        /*
         enable = true =>  판정X
         */
    }
    private void Update()
    {
        //UI라서 로컬 포지션으로 받아야함
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }
}
