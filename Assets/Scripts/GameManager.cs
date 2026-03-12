using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Manages the core game loop: timer, score tracking, and end-state UI.
public class GameManager : MonoBehaviour
{
    // Simple Singleton pattern for easy global access (e.g., from Zombie scripts)
    public static GameManager Instance;

    [Header("Game Settings")]
    public int score = 0;
    public float gameTime = 60f; // Configurable in Inspector per requirements

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (gameEnded) return;

        // Handle the countdown timer
        gameTime -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(gameTime).ToString();

        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    void EndGame()
    {
        gameEnded = true;
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;

        // Freeze the game world physics and updates when time runs out
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Must reset time scale to normal before reloading, otherwise the new scene starts paused
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}