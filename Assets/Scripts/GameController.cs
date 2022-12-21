using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int score;
    public TMP_Text scoreText;

    private void Awake()
    {
        instance = this;
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = "x " +  score.ToString();
    }
}
