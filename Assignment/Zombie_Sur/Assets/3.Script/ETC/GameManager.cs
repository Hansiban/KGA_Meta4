using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //½Ì±ÛÅæ ÇüÅÂ 2
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Destroy(gameObject);
        }
    }

    public int Score = 0;
    public bool isGameover { get; private set; }
    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDead += EndGame;
    }
    public void EndGame()
    {
        isGameover = true;
        UIController.instance.SetActive_Gameover(true);
    }
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            Score += newScore;
            UIController.instance.Update_Scoretext(Score);
        }
    }

}
