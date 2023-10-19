using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text recordText;

    private float time;
    private bool isGameover;

    private void Start()
    {
        time = 0;
        isGameover = false;
    }

    private void Update()
    {
        if (!isGameover)
        {
            //시간흘러감 ui update
            time += Time.deltaTime;
            timeText.text = $"Time : {(int)time}";
        }
        else
        {
            //재시작
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    public void EndGame()
    {
        isGameover = true;
        gameoverText.SetActive(true);

        float BestTime = PlayerPrefs.GetFloat("bestTime");
        if (time > BestTime)
        {
            BestTime = time;
            PlayerPrefs.SetFloat("bestTime", BestTime);
        }

        recordText.text = $"최고기록 : {(int)BestTime}";
    }
}
