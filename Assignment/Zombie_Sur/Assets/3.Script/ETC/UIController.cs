using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /*
     탄약 표시용 텍스트
     점수 표시 텍스트 -> Gamemanager -> r관리
     적 웨이브
     게임오버 프로젝트
     */
    [SerializeField] private Text Ammotext;
    [SerializeField] private Text Scoretext;
    [SerializeField] private Text Wavetext;

    [SerializeField] private GameObject Gameover_ob;

    //탄약 업데이트
    public void Update_Ammotext(int magAmmo, int Remain)
    {
        Ammotext.text = string.Format("{0} / {1}", magAmmo, Remain);
    }

    public void Update_Scoretext(int newScore)
    {
        //Score : 00
        Scoretext.text = string.Format("Score : {0}", newScore);
    }
    public void Update_Wavetext(int Wave, int Count)
    {
        //Wave : 0
        Wavetext.text = string.Format("Wave : {0}\n Zombie Left : {1}", Wave,Count);
    }
    public void SetActive_Gameover(bool isAct)
    {
        Gameover_ob.SetActive(isAct);
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Gameover_ob.SetActive(false);
    }

}
