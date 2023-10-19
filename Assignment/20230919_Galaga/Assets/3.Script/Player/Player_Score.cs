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
            �� �̵��� ���ؼ�, ������ �����ϴ� ���
            key - value������ ����
            ���� Ű�� �����Ѵٸ�, �ٽ� ���
            �ҷ��ö��� PlayerPrefs.GetIn("SCORE")
         */
        PlayerPrefs.SetInt("SCORE", player_Score);
    }
}
