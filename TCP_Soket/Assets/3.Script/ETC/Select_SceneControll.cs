using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select_SceneControll : MonoBehaviour
{
    public void SceneLoad(string name)
    {
        SceneManager.LoadScene(name);
    }
}
