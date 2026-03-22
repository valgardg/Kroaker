using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;
    Rigidbody2D rb;
    public float startSpeed = 2f;
    [SerializeField] private bool followPlayer = false;
    [SerializeField] private float crosshairSpeed;

    void Start()
    {
        crosshairSpeed = startSpeed;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!followPlayer) { return; }
        Vector3 playerDirection = playerTransform.position - transform.position;
        rb.linearVelocity = playerDirection.normalized * crosshairSpeed;
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        crosshairSpeed += speedIncrease;
    }

    public void StopFollowPlayer()
    {
        followPlayer = false;
        rb.linearVelocity = Vector3.zero;
    }

    public void StartFollowPlayer()
    {
        followPlayer = true;
    }
}