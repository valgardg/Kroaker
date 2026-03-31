using UnityEngine;

public class FieldBulletSpawner : MonoBehaviour
{
    public static FieldBulletSpawner Instance { get; private set; }
    [Header("Prefab")]
    public GameObject timedShotPrefab;

    [Header("Spawn Settings")]
    public float initialSpawnIndex = 3; // level index at which to start spawning
    public float spawnInterval = 8f;
    public int spawnCount = 1;  // Number of shots to spawn each interval
    public float shotDelay = 1f;
    public float rotationRangeDegrees = 45f;  // random Z rotation within +/- this range

    [Header("TimedShot Defaults")]
    public float defaultWaitSeconds = 1.5f;
    public float defaultBulletSpeed = 10f;

    private float nextSpawnTime;
    private bool spawningActive = false;

    [Header("Bullet Spawn Points")]
    public Transform[] spawnPoints;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple FieldBulletSpawner instances detected. There should only be one.");
            Destroy(this);
        }
    }

    void Update()
    {
        if (GameManager.Instance.GetGameOver())
            return;
        
        if (!spawningActive)
            return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnShots();
        }
    }

    // Spawn a batch of shots separated by shotDelay
    private void SpawnShots()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float delay = i * shotDelay;
            Invoke(nameof(SpawnTimedShot), delay);
        }
        // Wait for the last bullet's delay before starting the next interval
        float totalDelay = (spawnCount - 1) * shotDelay;
        nextSpawnTime = Time.time + spawnInterval + totalDelay;
    }

    private void SpawnTimedShot()
    {
        if (GameManager.Instance != null && GameManager.Instance.GetGameOver())
            return;

        // GameObject instance = Instantiate(timedShotPrefab, transform.position, spawnRot);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject instance = Instantiate(timedShotPrefab, spawnPoint.position, Quaternion.identity);

        TimedShot ts = instance.GetComponent<TimedShot>();
        if (ts != null)
        {
            // Override timing/speed; prefab holds path and bullet references
            ts.Initialize(defaultWaitSeconds, defaultBulletSpeed);
        }
        else
        {
            Debug.LogWarning("FieldBulletSpawner: Spawned prefab has no TimedShot component.");
        }
    }

    public void IncreaseDifficulty(int levelIndex)
    {
        if (levelIndex >= initialSpawnIndex && !spawningActive) // start spawning at initialSpawnIndex
        {
            StartSpawning();
        }
        if (spawningActive && (levelIndex - initialSpawnIndex) % 3 == 0) // every 3 levels, increase spawn count
        {
            spawnCount += 1;
        } else {
            spawnInterval = Mathf.Max(1f, spawnInterval - 0.5f); // decrease interval but not below 1 second
            defaultBulletSpeed += 1f; // increase bullet speed
            defaultWaitSeconds = Mathf.Max(0.5f, defaultWaitSeconds - 0.1f); // decrease wait time but not below 0.5 seconds

        }
    }

    private void StartSpawning()
    {
        initialSpawnIndex = 2f;
        nextSpawnTime = Time.time + initialSpawnIndex; // initial delay before first spawn
        spawningActive = true;
    }
}
