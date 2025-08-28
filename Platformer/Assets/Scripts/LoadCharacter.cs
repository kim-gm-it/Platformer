
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public int playerId = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt($"P{playerId}_SelectedCharacter", 0);

        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "level2")
        {
            selectedCharacter += 2;
        }
        else if(sceneName == "level3")
        {
            selectedCharacter += 4;
        }
        
        
        GameObject prefab = characterPrefabs[selectedCharacter];
        spawnPoint = prefab.transform;
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

    }


}
