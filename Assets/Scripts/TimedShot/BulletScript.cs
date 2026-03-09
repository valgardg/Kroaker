using UnityEngine;

public class BulletScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.CallHandleGameOver();
            Destroy(gameObject);
        }
    }
}
