using System.Collections;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private float minDirectionDuration = 0.4f;
    [SerializeField] private float maxDirectionDuration = 1.5f;
    [SerializeField] private float maxTurnAngle = 90f;

    private float targetAngle;

    void Start()
    {
        targetAngle = transform.eulerAngles.z;
        StartCoroutine(ChooseDirections());
    }

    void Update()
    {
        AvoidScreenEdges();

        float currentAngle = transform.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            turnSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0, 0, newAngle);

        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void AvoidScreenEdges()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        bool nearEdge =
            viewportPos.x < 0.1f ||
            viewportPos.x > 0.9f ||
            viewportPos.y < 0.1f ||
            viewportPos.y > 0.9f;

        if (nearEdge)
        {
            Vector3 screenCenter = Camera.main.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, viewportPos.z)
            );

            Vector2 directionToCenter =
                (screenCenter - transform.position).normalized;

            targetAngle =
                Mathf.Atan2(directionToCenter.y, directionToCenter.x)
                * Mathf.Rad2Deg - 90f;
        }
    }

    private IEnumerator ChooseDirections()
    {
        while (true)
        {
            float randomTurn = Random.Range(-maxTurnAngle, maxTurnAngle);

            targetAngle = transform.eulerAngles.z + randomTurn;

            float duration = Random.Range(minDirectionDuration, maxDirectionDuration);
            yield return new WaitForSeconds(duration);
        }
    }
}