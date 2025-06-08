using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] collectibles;             
    public float spawnInterval = 17f;          

    private Vector2 spawnAreaMin;             
    private Vector2 spawnAreaMax;             
    public float groundY = -2.5f;             
    public float margin = 0.5f;     
    public Camera targetCamera;         
    void Start()
    {
        float camHeight = targetCamera.orthographicSize;
        float camWidth = camHeight * targetCamera.aspect;
        
        spawnAreaMin = new Vector2(-camWidth + margin, groundY + margin);
        spawnAreaMax = new Vector2(camWidth - margin, camHeight - margin);

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
