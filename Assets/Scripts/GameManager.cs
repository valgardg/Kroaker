using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Jump playerJump;
    public CheckPlayerInCrosshair checkPlayerInCrosshair;
    public FollowPlayer crosshairFollowPlayer;
    public LevelText levelText;
    public ScoreText scoreText;
    public GameOverPanel gameOverPanel;

    public int ShotsBetweenIncrease = 5;
    [SerializeField] private int shotIndex = 0;
    [SerializeField] private float baseTimeBetweenShots = 5f;
    [SerializeField] private float baseTBSvariance = 0.3f;
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
        StartCoroutine(StartCrosshairLoop());
    }

    IEnumerator StartCrosshairLoop()
    {
        levelText.DisplayLevel(levelIndex);
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(CrosshairLoop());
    }

    IEnumerator CrosshairLoop()
    {
        while (!gameOver)
        {
            AudioManager.Instance.PlayReload();
            crosshairFollowPlayer.StartFollowPlayer();
            yield return new WaitForSeconds(GenerateTimeBetweenShots());
            crosshairFollowPlayer.StopFollowPlayer();
            
            // first check if player was shot 
            if(ShootGun())
            {
                break;
            }

            // update game state after shot has been fired
            if (shotIndex >= ShotsBetweenIncrease)
            {
                NewLevel();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private bool ShootGun()
    {
        // dont shoot gun if game is over
        if (gameOver) {  return true; }

        AudioManager.Instance.PlaySniperShot();
        bool playerInCrossHair = checkPlayerInCrosshair.GetPlayerInCrosshair();
            
        // first check if player was shot 
        if(playerInCrossHair)
        {
            HandleGameOver();
            return true;
        }

        // if not shot, increment shot index
        shotIndex++; // COUNT THE SHOT
        AddScore(1);
        
        return false;
    }

    private void NewLevel()
    {
        crosshairFollowPlayer.IncreaseSpeed();
        // timeBetweenShots -= 0.4f;
        shotIndex = 0;
        levelIndex += 1;
        int levelScore = 5;
        AddScore(levelScore);
        levelText.DisplayLevel(levelIndex, levelScore);
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
        crosshairFollowPlayer.StopFollowPlayer();
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.DisplayGameOverPanel(levelIndex, playerScore);
    }

    private float GenerateTimeBetweenShots()
    {
        return Random.Range(
            baseTimeBetweenShots - baseTBSvariance,
            baseTimeBetweenShots + baseTBSvariance
        );
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

    public void CallShootGun()
    {
        ShootGun();
    }
}
