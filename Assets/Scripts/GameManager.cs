using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Jump playerJump;
    public LevelText levelText;
    public LevelLabel LevelLabel;
    public ScoreText scoreText;
    public GameOverPanel gameOverPanel;

    private int levelIndex = 1;
    private int playerScore = 0;
    private bool gameOver = false;

    // LEVEL PROGRESSION
    // progression will depend on number of *crosshair* shots fired
    public int shotIndex = 0;
    private int shotsToNextLevel = 5;
    private int shotsToNextLevelIncrease = 3;

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
        levelText.DisplayLevel(levelIndex, 0); // display initial level
    }

    void Update()
    {
        if (gameOver)
            return;

        // Handle level progression through crosshair shots
        if (shotIndex >= shotsToNextLevel)
        {
            NewLevel();
            shotIndex = 0;
            shotsToNextLevel += shotsToNextLevelIncrease;
        }
    }

    private void NewLevel()
    {
        // Handle level info
        levelIndex += 1;
        int levelScore = 5;
        AddScore(levelScore);
        levelText.DisplayLevel(levelIndex, levelScore); // display level and score gained from level up
        // update level label
        LevelLabel.DisplayLevel(levelIndex);

        // Call increase difficulty
        CrosshairManager.Instance.IncreaseDifficulty();
        FieldBulletSpawner.Instance.IncreaseDifficulty(levelIndex);
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
