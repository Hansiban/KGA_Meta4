using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Score : MonoBehaviour
{
    private int player_Score = 0;
    public int Player_score => player_Score;

    [SerializeField] private Text Score;

    private void Awake()
    {
        player_Score = 0;
    }
    public void Set_Score(int score)
    {
        player_Score += score;
        Score.text = $"SCORE = {player_Score}";
    }

    public void SaveScore()
    {
        /*
         PlayerPrefs?
            씬 이동을 위해서, 변수를 저장하는 장소
            key - value값으로 저장
            만약 키가 존재한다면, 다시 덮어씀
            불러올때는 PlayerPrefs.GetIn("SCORE")
         */
        PlayerPrefs.SetInt("SCORE", player_Score);
    }
}
