using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] Transform visual;   // drag the Visual child here
    [SerializeField] float maxArcHeight = 0.8f;

    float flightTimer;
    float currentFlightDuration;
    Vector3 visualStartLocalPos;

    // throwing trash related variables
    public float chargeSpeed = 2f;
    public float baseCharge = 1.5f;
    public float charge;


    public float maxThrowForce = 4f;
    public float flightTime = 0.35f;

    bool isGrounded = true;
    Rigidbody2D rb;

    // boolean value to enable game manager
    // to stop player movement
    private bool allowJump = true;

    void Awake()
    {
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
        }
    }

    private void HandleInput()
    {
        if (!isGrounded || !allowJump)
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            charge += chargeSpeed * Time.deltaTime;
            charge = Mathf.Clamp(charge, 0f, maxThrowForce);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            AudioManager.Instance.PlayPlayerJump();
            Throw(charge);
            charge = baseCharge;
        }
    }

    public void Throw(float throwCharge)
    {
        isGrounded = false;
        rb.linearDamping = 0f;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;

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
