using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDirection : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    void Start()
    {
        rb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float xDirection = rb.linearVelocityX;
        if(xDirection > 0)
        {
            sr.flipX = false;
        } else if (xDirection < 0)
        {
            sr.flipX = true;
        }
    }
}
