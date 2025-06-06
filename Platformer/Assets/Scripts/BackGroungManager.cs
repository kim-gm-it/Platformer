using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BackGroungManager : MonoBehaviour
{
    [Header("Background Settings")]
    public GameObject backgroundPrefab;
    public Transform player;
    public float spawnAheadDis = 20f;

    private List<GameObject> activeBGs = new List<GameObject>();
    private float nextBGPosX = 0f; //x position for hte next bg to be spawned

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnInitialBackgrounds();
    }


    public void spawnInitialBackgrounds()
    {
        float viewWidth  = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float coverdWidth = 0f;

        while (coverdWidth < viewWidth + spawnAheadDis)
        {
            GameObject bg = spawnBackground();
            SpriteRenderer renderer = bg.GetComponent<SpriteRenderer>();
            coverdWidth += renderer.bounds.size.x;
        }
    }

    private void checkAndSpawnBackgrounds()
    {
        if (activeBGs.Count == 0)
            return;
        GameObject lastBG = activeBGs[activeBGs.Count - 1]; 
        SpriteRenderer lastRenderer = lastBG.GetComponent<SpriteRenderer>();

        float lastBGRightEdge = lastBG.transform.position.x + (lastRenderer.bounds.size.x / 2);
        float playerViewDis = player.position.x + Camera.main.orthographicSize * Camera.main.aspect * spawnAheadDis;
        if(lastBGRightEdge < playerViewDis)
        {
            spawnBackground();
        }
    }

    private GameObject spawnBackground()
    {
        if(backgroundPrefab == null)
        {
            return null;
        }

        GameObject neweBackground = Instantiate(backgroundPrefab, transform);
        SpriteRenderer renderer = neweBackground.GetComponent<SpriteRenderer>();

        if(activeBGs.Count == 0)
        {
            neweBackground.transform.position = new Vector3(0, 0, 0);
            nextBGPosX = renderer.bounds.size.x / 2;
        }
        else
        {
            float newBGWidth = renderer.bounds.size.x;
            float positionX = nextBGPosX + (newBGWidth / 2);
            neweBackground.transform.position = new Vector3(positionX , 0, 0);
            nextBGPosX += newBGWidth ;
        }
        activeBGs.Add(neweBackground);
        return neweBackground;
    }
    // Update is called once per frame
    void Update()
    {
        checkAndSpawnBackgrounds();
    }
}
