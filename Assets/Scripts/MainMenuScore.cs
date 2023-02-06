using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuScore : MonoBehaviour
{
    public GameObject highscoreObject;
    public TMP_Text scoreText;
    void Start()
    {
        UpdateScore();
    } 

    public void UpdateScore()
    {
        int score = FileSaving.GetScoreFromFile();

        if (score == 0 || score == -1)
        {
            highscoreObject.SetActive(false); 
        }
        else
        {
            scoreText.text = score.ToString(); 
            highscoreObject.SetActive(true);
        }
    }

    public void ResetScore()
    {
        FileSaving.SetScoreToFile(0);
        UpdateScore();
    }
}
