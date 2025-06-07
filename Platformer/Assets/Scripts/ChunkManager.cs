using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk settings")]
    [SerializeField] private float generateAheadDistance = 30f;
    [SerializeField] private int maxChunksVisible = 2;
    [SerializeField] private int initialChunks = 1;
    private float chunkWidth;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject lastChunk;

    private List<GameObject> activeChunks = new List<GameObject>();
    private List<int> randomizedChunkOrder = new List<int>();

    private int numOfCreatedChunks = 0;
    private float lastChunkX = 0f;
    private bool isLastChunkSpawned = false;

    void Start()
    {
        if (chunkPrefabs.Length < 5)
        {
            Debug.LogError("You must assign exactly 5 chunk prefabs!");
            return;
        }

        randomizedChunkOrder = GenerateShuffledIndexList(chunkPrefabs.Length);

        for (int i = 0; i < initialChunks; i++)
        {
            GameObject newChunk = GenerateChunk();
            activeChunks.Add(newChunk);
        }
    }

    void Update()
    {
        if (numOfCreatedChunks < 5)
        {
            float playerPosX = player.position.x;

            if (playerPosX + generateAheadDistance > lastChunkX)
            {
                GameObject newChunk = GenerateChunk();
                if(newChunk != null)
                {
                    activeChunks.Add(newChunk);
                }
            }

            if (activeChunks.Count > maxChunksVisible)
            {
                GameObject oldestChunk = activeChunks[0];
                activeChunks.RemoveAt(0);
                Destroy(oldestChunk);
            }
        }
        else if (!isLastChunkSpawned)
        {
            SpawnLastChunk();
        }
    }

    GameObject GenerateChunk()
    {
        if(numOfCreatedChunks >= randomizedChunkOrder.Count)
        {
            Debug.Log("all randomized chunks have been used. no more chunks to be generated");
            return null;
        }
        int chunkPrefabIndex = randomizedChunkOrder[numOfCreatedChunks];
        GameObject selectedChunk = chunkPrefabs[chunkPrefabIndex];
        GameObject newChunk = Instantiate(selectedChunk, transform);

        chunkWidth = newChunk.GetComponent<Renderer>().bounds.size.x;
        newChunk.transform.position = new Vector3(lastChunkX, 0, 0);
        lastChunkX += chunkWidth;

        numOfCreatedChunks++;
        return newChunk;
    }

    void SpawnLastChunk()
    {
        GameObject chunk = Instantiate(lastChunk, transform);
        chunk.transform.position = new Vector3(lastChunkX, 0, 0);
        isLastChunkSpawned = true;
    }

    List<int> GenerateShuffledIndexList(int count)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++)
        {
            list.Add(i);
        }

        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }

        return list;
    }
}
