using UnityEngine;

public class TimedShot : MonoBehaviour
{
    public GameObject bulletPath;
    public GameObject bulletObject;
    public float waitSeconds = 1f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 5f;

    private bool isMoving = false;
    private float moveStartTime = 0f;
    private bool shotScheduled = false;

    void Start()
    {
        if (!shotScheduled)
        {
            shotScheduled = true;
            StartCoroutine(FireAfterDelay());
        }
    }

    void Update()
    {
        if (!isMoving)
            return;

        if (bulletObject == null)
            return;

        // Move forward in the bullet's current facing direction (2D local right)
        bulletObject.transform.Translate(bulletObject.transform.up * bulletSpeed * Time.deltaTime, Space.World);

        if (Time.time - moveStartTime >= bulletLifetime)
        {
            Destroy(bulletObject);
            isMoving = false;
        }
    }

    System.Collections.IEnumerator FireAfterDelay()
    {
        if (waitSeconds > 0f)
        {
            yield return new WaitForSeconds(waitSeconds);
        }

        if (bulletPath != null)
        {
            bulletPath.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TimedShot: bulletPath is not assigned.");
        }

        if (bulletObject == null)
        {
            Debug.LogWarning("TimedShot: bulletObject is not assigned.");
            yield break;
        }

        isMoving = true;
        AudioManager.Instance.PlayPistolShot();
        moveStartTime = Time.time;
    }

    public void Initialize(float waitSeconds, float bulletSpeed)
    {
        this.waitSeconds = waitSeconds;
        this.bulletSpeed = bulletSpeed;

        isMoving = false;
        moveStartTime = 0f;

        StopAllCoroutines();
        shotScheduled = true;
        StartCoroutine(FireAfterDelay());
    }
}
