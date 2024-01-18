using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerName : MonoBehaviour
{
    public string input;
    public void GameStart()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void ReadInput(string s)
    {
        input = s;
        Debug.Log(input);
    }
}
