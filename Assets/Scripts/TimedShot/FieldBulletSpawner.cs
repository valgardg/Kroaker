using UnityEngine;

public class FieldBulletSpawner : MonoBehaviour
{
    public static FieldBulletSpawner Instance { get; private set; }
    [Header("Prefab")]
    public GameObject timedShotPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public int spawnCount = 1;  // Number of shots to spawn each interval
    public float shotDelay = 1f;
    public float rotationRangeDegrees = 45f;  // random Z rotation within +/- this range

    [Header("TimedShot Defaults")]
    public float defaultWaitSeconds = 0.5f;
    public float defaultBulletSpeed = 10f;
    public float defaultBulletLifetime = 5f;

    private float nextSpawnTime = 0f;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
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
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void SpawnTimedShot()
    {
        if (GameManager.Instance != null && GameManager.Instance.GetGameOver())
            return;
        float randRotZ = Random.Range(-rotationRangeDegrees, rotationRangeDegrees);
        Quaternion spawnRot = Quaternion.Euler(0f, 0f, randRotZ);

        GameObject instance = Instantiate(timedShotPrefab, transform.position, spawnRot);

        TimedShot ts = instance.GetComponent<TimedShot>();
        if (ts != null)
        {
            // Override timing/speed/lifetime; prefab holds path and bullet references
            ts.Initialize(defaultWaitSeconds, defaultBulletSpeed, defaultBulletLifetime);
        }
        else
        {
            Debug.LogWarning("FieldBulletSpawner: Spawned prefab has no TimedShot component.");
        }
    }
}
