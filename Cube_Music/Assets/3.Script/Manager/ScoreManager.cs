using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text score_text;
    [SerializeField] private Animator ani;

    private string aniKey = "Score";

    int current_Score = 0;
    int default_Score = 10;

    [SerializeField] private float[] weight = null;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        score_text = transform.GetChild(0).GetComponent<Text>();
    }

    public void AddScore(int index)
    {
        int _score = (int)(default_Score * weight[index]);
        current_Score += _score;
        score_text.text = string.Format("{0:#,##0}", current_Score);
        ani.SetTrigger(aniKey);
    }
}
