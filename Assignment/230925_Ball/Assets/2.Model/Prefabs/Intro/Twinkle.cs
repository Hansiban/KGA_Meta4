using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twinkle : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    private Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
        StartCoroutine(Twinkle_co());
    }
    private IEnumerator Twinkle_co()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));
            //이 코루틴이 끝날 때 까지는 다음 코루틴으로 넘어가지 않음
            yield return StartCoroutine(Fade(0, 1));
        }
    }
    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;
            yield return null; //한프레임씩
            
        }
    }
}
