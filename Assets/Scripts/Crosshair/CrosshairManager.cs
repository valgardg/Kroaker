using System.Collections;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Instance;
    public CheckPlayerInCrosshair checkPlayerInCrosshair;
    public FollowPlayer crosshairFollowPlayer;

    [SerializeField] private float baseTimeBetweenShots = 5f;
    private float baseTBSvariance = 0.4f;
    private float timeBetweenShots;
    [SerializeField] private float baseTimeBetweenReload = 2f;
    private float timeBetweenReload;
    private float baseTBRvariance = 0.2f;

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
        timeBetweenShots = baseTimeBetweenShots;
        timeBetweenReload = baseTimeBetweenReload;
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(CrosshairLoop());
    }

    IEnumerator CrosshairLoop()
    {
        while (!GameManager.Instance.GetGameOver())
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

            yield return new WaitForSeconds(GenerateTimeBetweenReload());
        }
    }

    public bool ShootGun()
    {
        // dont shoot gun if game is over
        if (GameManager.Instance.GetGameOver()) {  return true; }

        AudioManager.Instance.PlaySniperShot();
        bool playerInCrossHair = checkPlayerInCrosshair.GetPlayerInCrosshair();
            
        // first check if player was shot 
        if(playerInCrossHair)
        {
            GameManager.Instance.CallHandleGameOver();
            return true;
        }

        // if player was not shot, add score and increase shot index for level progression
        GameManager.Instance.CallAddScore(1);
        GameManager.Instance.shotIndex += 1;
        
        return false;
    }

    private float GenerateTimeBetweenShots()
    {
        return Random.Range(
            timeBetweenShots - baseTBSvariance,
            timeBetweenShots + baseTBSvariance
        );
    }

    private float GenerateTimeBetweenReload()
    {
        return Random.Range(
            timeBetweenReload - baseTBRvariance,
            timeBetweenReload + baseTBRvariance
        );
    }

    // CROSSHAIR LEVEL UP
    public void IncreaseDifficulty()
    {
        timeBetweenShots = Mathf.Max(1.0f, timeBetweenShots - 0.4f);
        timeBetweenReload = Mathf.Max(0.5f, timeBetweenReload - 0.1f);
        crosshairFollowPlayer.IncreaseSpeed(increaseSpeedAmount);
    }
}
