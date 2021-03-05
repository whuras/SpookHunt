using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScoreManagement : MonoBehaviour
{
    // High Score Management
    public string[] highScoreNames;
    public int[] highScores;
    public Text scoreText;
    public Text nameText;

    private void Awake()
    {
        highScores = new int[6];
        highScoreNames = new string[6];

        for (int i = 0; i < highScores.Length; i++)
        {
            highScores[i] = PlayerPrefs.GetInt("Highscores" + i);
            highScoreNames[i] = PlayerPrefs.GetString("HighScoreNames" + i);
        }

        scoreText.text = "SCORE \n" +
            highScores[0] + "\n" +
            highScores[1] + "\n" +
            highScores[2] + "\n" +
            highScores[3] + "\n" +
            highScores[4] + "\n";

        nameText.text = "NAME \n" +
            highScoreNames[0] + "\n" +
            highScoreNames[1] + "\n" +
            highScoreNames[2] + "\n" +
            highScoreNames[3] + "\n" +
            highScoreNames[4] + "\n";

    }

}
