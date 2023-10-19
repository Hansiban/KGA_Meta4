using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance = null;

    /*
    ��𼭵� ���� �����ؾ��ϰ�
    � ������Ʈ�� �����Ͽ� ����ؾ���
    Gameover, score�� ������ ���̱� ����
    => �̱��� ������ �ʿ���
     */
    private void Awake()
    {
        //�̱��� �⺻ ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("�̹� ���� �Ŵ����� �����մϴ�.");
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
            //restart �� ������� �ٽ� �ε�
            SceneManager.LoadScene(0);
        }
        if (Score >= 3 && SceneManager.GetActiveScene().name == "stage1")
        {
            //���ھ� 3�� �̻� �� ���� ���� �ε�
            SceneManager.LoadScene(1);
        }
        if (Score >= 6 && SceneManager.GetActiveScene().name == "stage2")
        {
            //���ھ� 6�� �̻� �� Ŭ���� ���
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
