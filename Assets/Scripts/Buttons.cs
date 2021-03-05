using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public Text text;
    private float timer;
    
    void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer >= 0.5 && text)
        {
            text.enabled = true;
        }
        if (timer >= 1 && text)
        {
            text.enabled = false;
            timer = 0;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
