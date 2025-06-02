using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] collectibles; 
    public Tilemap[] tilemaps;
    public float spawnInterval = 15f;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    void Start()
    {
        InvokeRepeating("SpawnCollectible", 1f, spawnInterval);
    }

    void SpawnCollectible()
{
    if (tilemaps.Length == 0 || collectibles.Length == 0) return;

    Tilemap tilemap = tilemaps[Random.Range(0, tilemaps.Length)];
    BoundsInt bounds = tilemap.cellBounds;

    for (int attempt = 0; attempt < 10; attempt++)
    {
        Vector3Int randomCell = new Vector3Int(
            Random.Range(bounds.xMin, bounds.xMax),
            Random.Range(bounds.yMin, bounds.yMax),
            0
        );

        if (tilemap.HasTile(randomCell))
        {
            Vector3 worldPos = tilemap.CellToWorld(randomCell) + tilemap.cellSize / 2f;

            Instantiate(
                collectibles[Random.Range(0, collectibles.Length)],
                worldPos,
                Quaternion.identity
            );
            return;
        }
    }
}

}