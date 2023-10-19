using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance = null;

    /*
    어디서든 접근 가능해야하고
    어떤 오브젝트든 접근하여 사용해야함
    Gameover, score를 관리할 것이기 때문
    => 싱글톤 구현이 필요함
     */
    private void Awake()
    {
        //싱글톤 기본 형태
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("이미 게임 매니저가 존재합니다.");
            Destroy(gameObject);
        }
    }
    public bool isGameover = false;
    public bool isClear = false;

    public GameObject GameoverUI;
    public GameObject GameClearUI;
    public Text Score_text;

    static public int Score = 0;
    private void Start()
    {
        Score_text.text = "Score : " + Score;
    }

    private void Update()
    {
        if ((isGameover || isClear ) && Input.GetMouseButtonDown(0))
        {
            //restart 시 현재씬을 다시 로드
            SceneManager.LoadScene(0);
        }
        if (Score >= 3 && SceneManager.GetActiveScene().name == "stage1")
        {
            //스코어 3점 이상 시 다음 씬을 로드
            SceneManager.LoadScene(1);
        }
        if (Score >= 6 && SceneManager.GetActiveScene().name == "stage2")
        {
            //스코어 6점 이상 시 클리어 출력
            Player_Clear();
        }
    }

    public void AddScore(int score)
    {
        if (!isGameover)
        {
            Score += score;
            Score_text.text = "Score : " + Score;
        }
    }
    public void Player_Dead()
    {
        isGameover = true;
        Score = 0;
        GameoverUI.SetActive(true);
    }

    public void Player_Clear()
    {
        isClear = true;
        GameClearUI.SetActive(true);
    }
}
