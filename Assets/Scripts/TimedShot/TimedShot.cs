using UnityEngine;

public class TimedShot : MonoBehaviour
{
    public GameObject bulletIndicator;
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

        if (gameObject == null)
            return;

        // Move forward in the bullet's current facing direction (2D local right)
        transform.Translate(transform.up * bulletSpeed * Time.deltaTime, Space.World);
        if (Time.time - moveStartTime >= bulletLifetime)
        {
            Destroy(gameObject);
            isMoving = false;
        }
    }

    System.Collections.IEnumerator FireAfterDelay()
    {
        if (waitSeconds > 0f)
        {
            yield return new WaitForSeconds(waitSeconds);
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

        Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        transform.up = direction;

        PlaceIndicator();

        StopAllCoroutines();
        shotScheduled = true;
        StartCoroutine(FireAfterDelay());
    }

    private void PlaceIndicator()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.up;

        Vector2 edgePoint = GetScreenEdgeIntersection(origin, direction);
        edgePoint += direction * 1f;

        GameObject indicator = Instantiate(bulletIndicator);
        indicator.transform.position = edgePoint;

        Vector2 toPlayer = (GameObject.FindGameObjectWithTag("Player").transform.position - (Vector3)edgePoint).normalized;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

        private Vector2 GetScreenEdgeIntersection(Vector2 origin, Vector2 direction)
    {
        Camera cam = Camera.main;

        // Get screen bounds in world space
        float z = Mathf.Abs(cam.transform.position.z);
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, z));

        float minX = bottomLeft.x;
        float maxX = topRight.x;
        float minY = bottomLeft.y;
        float maxY = topRight.y;

        direction.Normalize();

        float tMin = float.PositiveInfinity;

        // Check intersection with vertical edges
        if (direction.x != 0)
        {
            float t1 = (minX - origin.x) / direction.x;
            float t2 = (maxX - origin.x) / direction.x;

            if (t1 > 0)
            {
                float y = origin.y + t1 * direction.y;
                if (y >= minY && y <= maxY)
                    tMin = Mathf.Min(tMin, t1);
            }

            if (t2 > 0)
            {
                float y = origin.y + t2 * direction.y;
                if (y >= minY && y <= maxY)
                    tMin = Mathf.Min(tMin, t2);
            }
        }

        // Check intersection with horizontal edges
        if (direction.y != 0)
        {
            float t3 = (minY - origin.y) / direction.y;
            float t4 = (maxY - origin.y) / direction.y;

            if (t3 > 0)
            {
                float x = origin.x + t3 * direction.x;
                if (x >= minX && x <= maxX)
                    tMin = Mathf.Min(tMin, t3);
            }

            if (t4 > 0)
            {
                float x = origin.x + t4 * direction.x;
                if (x >= minX && x <= maxX)
                    tMin = Mathf.Min(tMin, t4);
            }
        }

        // Final intersection point
        return origin + direction * tMin;
    }
}
