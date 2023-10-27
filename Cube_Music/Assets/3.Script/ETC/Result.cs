using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private GameObject ui_object;
    [SerializeField] private Text[] text_count;
    [SerializeField] private Text score_text;
    [SerializeField] private Text maxcombo_text;

    //점수
    ScoreManager scoreManager;
    //maxcombo
    ComboManager comboManager;
    //판정 카운트
    TimingManager timingManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        timingManager = FindObjectOfType<TimingManager>();
        Initialized();
    }

    private void Initialized()
    {
        for (int i = 0; i < text_count.Length; i++)
        {
            text_count[i].text = "0";
        }
        score_text.text = "0";
        maxcombo_text.text = "0";
    }
    private string StringFormat(string s)
    {
        return string.Format("{0:#,##0}", s);
    }

    public void ShowResult()
    {
        //배경음악 스탑
        AudioManager.instance.StopBGM();
        Initialized();
        ui_object.SetActive(true);

        //판정기록 등등
        int[] record_arr = timingManager.Get_judgement_Record();
        for (int i = 0; i < text_count.Length; i++)
        {
            text_count[i].text = StringFormat(record_arr[i].ToString());
        }
        score_text.text = StringFormat(scoreManager.Get_Score().ToString());
        maxcombo_text.text = StringFormat(comboManager.GetMaxCombo().ToString());
    }

}
