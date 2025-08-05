using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs = new GameObject[2];
    public int maxEnemies = 6;
    public int minEnemies = 2;
    public float spawnRange = 5f ;
    
    public void SpawnEenmy()
    {
        int numOfEnemies = Random.Range(maxEnemies, minEnemies);
        int counter = 0;

        GameObject enemyMinion = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        while(counter++ <= numOfEnemies)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));

            Instantiate(enemyMinion, spawnPos, Quaternion.identity);
        }

    }
}
