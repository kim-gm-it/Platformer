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
        if (collectibles.Length == 0) return;

        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(x, y);

        Instantiate(
            collectibles[Random.Range(0, collectibles.Length)],
            spawnPosition,
            Quaternion.identity
        );
    }
}
