using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn varibales")]
    public GameObject[] enemyPrefabs = new GameObject[2];
    public int maxEnemies = 6;
    public int minEnemies = 2;
    public float spawnRange = 5f ;

    [Header("boss references")]
    public Transform bossTransform;
    public void SpawnEenmy()
    {
        int numOfEnemies = Random.Range(minEnemies, maxEnemies);
        int counter = 0;

        
        while(counter++ < numOfEnemies)
        {
            //random enemy 
            GameObject enemyMinion = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            //chose a random point within a circle aroung the boss
            Vector2 spawnPos = (Vector2)bossTransform.position + Random.insideUnitCircle * spawnRange;

            Instantiate(enemyMinion, spawnPos, Quaternion.identity);
        }

    }
}
