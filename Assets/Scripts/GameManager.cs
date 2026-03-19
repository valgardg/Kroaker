using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Jump playerJump;
    public LevelText levelText;
    public ScoreText scoreText;
    public GameOverPanel gameOverPanel;

    private int levelIndex = 1;
    private int playerScore = 0;
    private bool gameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        
    }

    private void NewLevel()
    {
        // Handle level info
        levelIndex += 1;
        int levelScore = 5;
        AddScore(levelScore);
        levelText.DisplayLevel(levelIndex, levelScore);

        // Call crosshair manager to increase difficulty
        CrosshairManager.Instance.IncreaseDifficulty();
    }

    private void ReloadCurrentScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.DisplayScore(playerScore);
    }

    private void HandleGameOver()
    {
        gameOver = true;
        playerJump.SetAllowJump(false);
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.DisplayGameOverPanel(levelIndex, playerScore);
    }

    public void Reset()
    {
        ReloadCurrentScene();
    }

    public void CallHandleGameOver()
    {
        HandleGameOver();
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void CallAddScore(int scoreToAdd)
    {
        AddScore(scoreToAdd);
    }
}
