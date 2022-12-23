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
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = "x " + score.ToString();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
