using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] Transform visual;   // drag the Visual child here
    [SerializeField] float maxArcHeight = 0.8f;

    float flightTimer;
    float currentFlightDuration;
    float groundedTime;
    Vector3 visualStartLocalPos;

    // charge variables
    public float chargeSpeed = 2f;
    public float baseCharge = 1.5f;
    public float charge;

    public float jumpCooldown = 0.1f; // minimum time between jumps


    public float maxThrowForce = 4f;
    public float flightTime = 0.35f;

    bool isGrounded = true;
    Rigidbody2D rb;

    bool useMouseInput = false;

    // boolean value to enable game manager
    // to stop player movement
    private bool allowJump = true;

    void Awake()
    {
        groundedTime = 0f;
        charge = baseCharge;
        rb = GetComponent<Rigidbody2D>();
        visualStartLocalPos = visual.localPosition;
    }

    void Update()
    {
        HandleInput();

        if(!isGrounded)
        {
            flightTimer += Time.deltaTime;
            float t = Mathf.Clamp01(flightTimer / currentFlightDuration);

            float height = 4f * t * (1f - t);
            height *= maxArcHeight;

            visual.localPosition = visualStartLocalPos + Vector3.up * height;
        } else
        {
            groundedTime += Time.deltaTime;
        }
    }

    private void HandleInput()
    {
        if (!isGrounded || !allowJump || groundedTime < jumpCooldown)
        {
            return;
        }

        if (useMouseInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.Instance.PlayPlayerJump();

                Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;

                Jump(charge, direction);
                charge = baseCharge;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                charge += chargeSpeed * Time.deltaTime;
                charge = Mathf.Clamp(charge, 0f, maxThrowForce);
                return;
            }

            Vector2 direction = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            // Jump when WASD is first pressed
            if (direction != Vector2.zero)
            {
                AudioManager.Instance.PlayPlayerJump();

                Jump(charge, direction.normalized);
                charge = baseCharge;
            }
        }
    }

    public void Jump(float throwCharge, Vector2 direction)
    {
        isGrounded = false;
        rb.linearDamping = 0f;

        float force = throwCharge * maxThrowForce;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        CancelInvoke();

        currentFlightDuration =  Mathf.Lerp(0.05f, flightTime, throwCharge);
        flightTimer = 0;

        Invoke(nameof(Land), currentFlightDuration);
    }

    void Land()
    {
        isGrounded = true;
        groundedTime = 0f;

        visual.localPosition = visualStartLocalPos;

        Vector2 throwDirection = rb.linearVelocity.normalized;
        rb.linearVelocity = Vector2.zero;

        float skidForce = 2f;
        rb.linearDamping = 6f;
        rb.AddForce(throwDirection * skidForce, ForceMode2D.Impulse);
    }

    public void SetAllowJump(bool value)
    {
        allowJump = value;
    }
}
