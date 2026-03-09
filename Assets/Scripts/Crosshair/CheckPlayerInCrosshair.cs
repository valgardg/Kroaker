using System.Collections;
using UnityEngine;

public class CheckPlayerInCrosshair : MonoBehaviour
{
    private bool playerInCrosshair = false;
    private Coroutine quickShotCoroutine;
    private float baseQuickShotTTK = 1.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerInCrosshair = true;
            quickShotCoroutine = StartCoroutine(QuickShotRoutine());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerInCrosshair = false;
            StopCoroutine(quickShotCoroutine);
            quickShotCoroutine = null;
        }
    }

    private IEnumerator QuickShotRoutine()
    {
        float quickShotLoad = 0f;

        while (playerInCrosshair)
        {
            quickShotLoad += Time.deltaTime;

            if (quickShotLoad >= baseQuickShotTTK)
            {
                Debug.Log("QUICK SHOT KILL");
                GameManager.Instance.CallShootGun();
                yield break;
            }

            yield return null; // wait one frame
        }
    }

    public bool GetPlayerInCrosshair()
    {
        return playerInCrosshair;
    }
}
