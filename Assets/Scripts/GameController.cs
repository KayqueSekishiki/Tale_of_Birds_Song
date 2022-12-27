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
    public GameObject gameOverPanel;

    public int score;
    public TMP_Text scoreText;


    private void Awake()
    {
        Time.timeScale = 1;
        instance = this;
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);
        //}
        //else if (instance != this)
        //{
        //    Destroy(instance.gameObject);
        //    instance = this;
        //    DontDestroyOnLoad(this);

        //}

        if (PlayerPrefs.GetInt("score") > 0)
        {

            score += PlayerPrefs.GetInt("score");
            scoreText.text = "x " + score.ToString();

            //if (score.ToString().Length == 1)
            //{
            //    scoreText.text = "x 000" + score.ToString();
            //}
            //else if (score.ToString().Length == 2)
            //{
            //    scoreText.text = "x 00" + score.ToString();
            //}

            //else if (score.ToString().Length == 3)
            //{
            //    scoreText.text = "x 0" + score.ToString();
            //}

            //else 
            //{
            //    scoreText.text = "x " + score.ToString();
            //}
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

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
