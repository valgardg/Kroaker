using System.Collections;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private bool hasDirection = false;
    private float directionDuration = 1.0f;
    private int direction = 0;

    void Update()
    {
        if (!hasDirection)
        {
            ChooseDirection();
        }

        float angle = Mathf.Atan2(direction, 1) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void ChooseDirection()
    {
        hasDirection = true;
        direction = Random.value < 0.5f ? -1 : 1;
        StartCoroutine(DirectionTimer());
    }

    IEnumerator DirectionTimer()
    {
        yield return new WaitForSeconds(directionDuration);
        hasDirection = false;
    }


}
