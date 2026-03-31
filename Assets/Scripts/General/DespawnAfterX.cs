using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float despawnTime = 5f;
    void Start()
    {
        StartCoroutine(DespawnAfterSeconds());
    }

    System.Collections.IEnumerator DespawnAfterSeconds()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
