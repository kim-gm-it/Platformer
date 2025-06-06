using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk settings")]
    [SerializeField] private float generateAheadDistance = 30f;
    [SerializeField] private int chunkWidth = 26;
    [SerializeField] private int maxChunksVisible = 2;
    [SerializeField] private int initialChunks = 1;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject lastChunk;

    private int currentChunkIndex = 0;
    private int numOfCreatedChunks = 0;
    private List<GameObject> activeChunks = new List<GameObject>();
    private float chunkWorldWidth;
    private bool isLastChunkSpawned = false;

    void Start()
    {
        if (chunkPrefabs.Length == 0)
        {
            Debug.LogError("No chunk prefabs assigned!");
            return;
        }

        chunkWorldWidth = chunkWidth;

        // Generate initial chunks
        for (int i = 0; i < initialChunks; i++)
        {
            GameObject newChunk = generateChunks(i);
            activeChunks.Add(newChunk);
            numOfCreatedChunks++;
        }

        currentChunkIndex = initialChunks;
    }

    void Update()
    {
        if (numOfCreatedChunks < 5)
        {
            float playerPosX = player.position.x;
            float furthestChunkEndX = (currentChunkIndex - 1 + activeChunks.Count) * chunkWorldWidth;

            if (playerPosX + generateAheadDistance > furthestChunkEndX)
            {
                GameObject newChunk = generateChunks(currentChunkIndex - 1 + activeChunks.Count);
                activeChunks.Add(newChunk);
                numOfCreatedChunks++;
            }

            if (activeChunks.Count > maxChunksVisible)
            {
                GameObject oldestChunk = activeChunks[0];
                activeChunks.RemoveAt(0);
                Destroy(oldestChunk);
                currentChunkIndex++;
            }
        }
        else if (!isLastChunkSpawned)
        {
            // Instantiate the last chunk
            GameObject chunk = Instantiate(lastChunk, transform);
            int chunkIndex = currentChunkIndex - 1 + activeChunks.Count;
            float xPos = chunkIndex * chunkWorldWidth;
            chunk.transform.position = new Vector3(xPos, 0, 0);
            isLastChunkSpawned = true;
        }
    }

    GameObject generateChunks(int index)
    {
        int randomChunkIndex = Random.Range(0, chunkPrefabs.Length);
        GameObject selectedChunk = chunkPrefabs[randomChunkIndex];
        GameObject newChunk = Instantiate(selectedChunk, transform);
        float xPos = index * chunkWorldWidth;
        newChunk.transform.position = new Vector3(xPos, 0, 0);
        return newChunk;
    }
}
