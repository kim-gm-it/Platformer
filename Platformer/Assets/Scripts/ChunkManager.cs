using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Security.Principal;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] static int numOfCreatedChunks = 0;

    [Header("Chunk settings")]
    [SerializeField] private float generateAheadDistance = 22f;
    [SerializeField] private int chunkWidth = 18;
    [SerializeField] private int maxChunksVisible = 3;
    [SerializeField] private int initialChunks = 2;

    [Header("Refrences")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject lastChunk;

    private int currentChunkIndex = 0;
    private List<GameObject> activeChunks = new List<GameObject>();
    private float chunkWorldWidth;

    void Start()
    {
        if (chunkPrefabs.Length == 0)
        {
            Debug.Log("No chunk prefabs assigned!");
        }
        chunkWorldWidth = chunkWidth;

        //generate initial chunks
        for (int i = 0; i < initialChunks; i++)
        {
            GameObject newChunk = generateChunks(i);
            activeChunks.Add(newChunk);
        }
        numOfCreatedChunks += activeChunks.Count;
    }

    void Update()
    {
        if (numOfCreatedChunks <= 5)
        {
            float playerPosX = player.position.x;
            float furthestChunkEndX = (currentChunkIndex - 1 + activeChunks.Count) * chunkWorldWidth;
            if (playerPosX + generateAheadDistance > furthestChunkEndX)
            {
                GameObject newChunk = generateChunks(currentChunkIndex - 1 + activeChunks.Count);
                activeChunks.Add(newChunk);
            }

            //Remove chunks that are far behind
            if (activeChunks.Count > maxChunksVisible)
            {
                GameObject oldestChunk = chunkPrefabs[0];
                activeChunks.RemoveAt(0);
                Destroy(oldestChunk);
                currentChunkIndex++;
            }
        }
        else
        {
            //Instantiate the last chunk
            GameObject chunk = Instantiate(lastChunk, transform);

            //Position the last chunk
            int chunkIndex = currentChunkIndex - 1 + activeChunks.Count;
            float xPos = chunkIndex * chunkWorldWidth;
            chunk.transform.position = new Vector3(xPos, 0, 0);

        }
    }

    GameObject generateChunks(int index)
    {
        //Randomly chose a chunk prefab from the array
        int randomChunkIndex = Random.Range(0, chunkPrefabs.Length);
        GameObject selectedChunk = chunkPrefabs[randomChunkIndex];

        //Instantiate the selected chunk
        GameObject newChunk = Instantiate(selectedChunk, transform);

        //Position the chunk
        float xPos = index * chunkWorldWidth;
        newChunk.transform.position = new Vector3(xPos, 0, 0);
        return newChunk;
        
    }
}
