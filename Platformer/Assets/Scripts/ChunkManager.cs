using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk settings")]
    [SerializeField] private float generateAheadDistance = 30f;
    // [SerializeField] private int maxChunksVisible = 2;
    //[SerializeField] private int maxChunksVisible = 2;
    [SerializeField] private int initialChunks = 1;
    [SerializeField] private float yoffset = 0;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private GameObject lastChunk;
    [Header("Collectible Settings")]
    [SerializeField] private GameObject[] collectiblePrefabs;
    [SerializeField] private int minCollectiblesPerChunk = 1;
    [SerializeField] private int maxCollectiblesPerChunk = 3;
    [SerializeField] private float collectibleYOffset = 1f;


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
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player1").transform;
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
                if (newChunk != null)
                {
                    activeChunks.Add(newChunk);
                }

            }

            // if (activeChunks.Count > maxChunksVisible)
            // {
            //     GameObject oldestChunk = activeChunks[0];
            //     activeChunks.RemoveAt(0);
            //     Destroy(oldestChunk);
            // }
        }
        else if (!isLastChunkSpawned)
        {
            SpawnLastChunk();
        }
    }

    GameObject GenerateChunk()
    {
        if (numOfCreatedChunks >= randomizedChunkOrder.Count)
        {
            Debug.Log("No more chunks available in randomized order");
            return null;
        }
        int chunkPrefabIndex = randomizedChunkOrder[numOfCreatedChunks];
        GameObject selectedChunk = chunkPrefabs[chunkPrefabIndex];
        GameObject newChunk = Instantiate(selectedChunk, transform);


        Transform startMark = newChunk.transform.Find("ChunkStart");
        Transform endMark = newChunk.transform.Find("ChunkEnd");

        float chunkWidth = endMark.position.x - startMark.position.x;
        float offset = startMark.position.x - newChunk.transform.position.x;

        if (chunkPrefabIndex == 0 || chunkPrefabIndex == 4)
        {
            yoffset = -8f;
        }
        newChunk.transform.position = new Vector3(lastChunkX - offset, yoffset, 0);

        lastChunkX += chunkWidth;

        numOfCreatedChunks++;
        SpawnCollectiblesInChunk(newChunk, startMark.position.x, endMark.position.x);

        return newChunk;
    }

    void SpawnLastChunk()
    {
        GameObject chunk = Instantiate(lastChunk, transform);

        Transform startMark = chunk.transform.Find("ChunkStart");
        Transform endMark = chunk.transform.Find("ChunkEnd");

        if (startMark == null || endMark == null)
        {
            Debug.LogError("Start or End marker not found on lastChunk prefab.");
            return;
        }

        float chunkWidth = endMark.position.x - startMark.position.x;
        float offset = startMark.position.x - chunk.transform.position.x;
        chunk.transform.position = new Vector3(lastChunkX - offset, -8, 0);
        lastChunkX += chunkWidth;
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
    void SpawnCollectiblesInChunk(GameObject chunk, float startX, float endX)
    {
        if (collectiblePrefabs.Length == 0) return;

        int collectibleCount = Random.Range(minCollectiblesPerChunk, maxCollectiblesPerChunk + 1);

        for (int i = 0; i < collectibleCount; i++)
        {
            float randomX = Random.Range(startX + 1f, endX - 1f); 
            float y = chunk.transform.position.y + collectibleYOffset;

            Vector3 spawnPosition = new Vector3(randomX, y, 0);

            GameObject collectible = Instantiate(
                collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)],
                spawnPosition,
                Quaternion.identity,
                chunk.transform 
            );
        }
    }

}