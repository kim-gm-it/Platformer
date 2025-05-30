using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] collectibles; 
    public float spawnInterval = 15f;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    void Start()
    {
        InvokeRepeating("SpawnCollectible", 1f, spawnInterval);
    }

    void SpawnCollectible()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(
            collectibles[Random.Range(0, collectibles.Length)],
            spawnPos,
            Quaternion.identity
        );
    }
}
