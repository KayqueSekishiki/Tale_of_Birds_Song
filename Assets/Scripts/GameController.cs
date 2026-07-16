using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private GameObject gameOverPanel;
    private TMP_Text scoreText;

    private int score;

    // Estado salvo no começo da tentativa
    private int savedScore;


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

        // Guarda o estado inicial da tentativa
        savedScore = score;
    }


    public void RegisterUI(GameObject panel, TMP_Text score)
    {
        gameOverPanel = panel;
        scoreText = score;

        gameOverPanel.SetActive(false);
        UpdateScoreUI();
    }


    // MOEDA TEMPORÁRIA DA TENTATIVA
    public void GetCoin()
    {
        score++;
        UpdateScoreUI();
    }


    // Salva somente quando termina a fase
    public void SaveProgress(int health, int heartsCount)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("health", health);
        PlayerPrefs.SetInt("heartsCount", heartsCount);

        PlayerPrefs.Save();

        // Atualiza o ponto de retorno
        savedScore = score;
    }

    public int LoadLevel()
    {
        return PlayerPrefs.GetInt("level", 0);
    }


    public int LoadHealth()
    {
        return PlayerPrefs.GetInt("health", 3);
    }


    public int LoadHeartsCount()
    {
        return PlayerPrefs.GetInt("heartsCount", 3);
    }


    // Chamado quando o jogador morre
    public void ResetAttempt()
    {
        score = savedScore;

        UpdateScoreUI();
    }


    public void ShowGameOver()
    {
        Time.timeScale = 0;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }


    public void RestartGame()
    {
        Time.timeScale = 1;

        // Perde moedas da tentativa
        ResetAttempt();

        // Recupera a vida inicial baseada nos corações conquistados
        int hearts = LoadHeartsCount();

        PlayerPrefs.SetInt("health", hearts);
        PlayerPrefs.Save();


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