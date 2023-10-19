using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Viewer : MonoBehaviour
{
    private Text score_text;

    private void Start()
    {
        TryGetComponent(out score_text);

        score_text.text = $"SCORE : {PlayerPrefs.GetInt("SCORE")}";
    }
}
