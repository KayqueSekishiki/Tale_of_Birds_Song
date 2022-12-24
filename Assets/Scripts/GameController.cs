using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int score;
    public TMP_Text scoreText;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("score") > 0)
        {

            score = PlayerPrefs.GetInt("score");

            if (score.ToString().Length == 1)
            {
                scoreText.text = "x 000" + score.ToString();
            }
            else if (score.ToString().Length == 2)
            {
                scoreText.text = "x 00" + score.ToString();
            }

            else if (score.ToString().Length == 3)
            {
                scoreText.text = "x 0" + score.ToString();
            }

            else 
            {
                scoreText.text = "x " + score.ToString();
            }
        }
    }

    public void GetCoin()
    {
        score++;


        if (score.ToString().Length == 1)
        {
            scoreText.text = "x 000" + score.ToString();
        }


        if (score.ToString().Length == 2)
        {
            scoreText.text = "x 00" + score.ToString();
        }

        if (score.ToString().Length == 3)
        {
            scoreText.text = "x 0" + score.ToString();
        }

        if (score.ToString().Length > 3)
        {
            scoreText.text = "x " + score.ToString();
        }

        PlayerPrefs.SetInt("score", score);

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
