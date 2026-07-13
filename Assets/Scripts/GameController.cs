using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private GameObject gameOverPanel;
    private TMP_Text scoreText;

    private int score;

    private void Awake()
    {
        Time.timeScale = 1;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        score = PlayerPrefs.GetInt("score", 0);
    }

    public void RegisterUI(GameObject panel, TMP_Text score)
    {
        gameOverPanel = panel;
        scoreText = score;

        gameOverPanel.SetActive(false);
        UpdateScoreUI();
    }

    public void GetCoin()
    {
        score++;

        UpdateScoreUI();

        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;

        gameOverPanel?.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"x {score:0000}";
    }
}