using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    [Header("Player References")]
    public BasePlayer player1;
    public BasePlayer player2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);

        PlayerPrefs.SetFloat("P1_HealthBar", player1.livesBar);
        PlayerPrefs.SetInt("P1_HealthPoints", player1.livesPoint);

        PlayerPrefs.SetFloat("P2_HealthBar", player2.livesBar);
        PlayerPrefs.SetInt("P2_HealthPoints", player2.livesPoint);

        PlayerPrefs.Save();
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavedScene"))
        {
            Debug.LogWarning("No saved game found!");
            return;
        }

        string sceneToLoad = PlayerPrefs.GetString("SavedScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        player1 = GameObject.FindWithTag("Player1").GetComponent<BasePlayer>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<BasePlayer>();

        player1.livesBar = PlayerPrefs.GetFloat("P1_HealthBar");
        player1.livesPoint = PlayerPrefs.GetInt("P1_HealthPoints");

        player2.livesBar = PlayerPrefs.GetFloat("P2_HealthBar");
        player2.livesPoint = PlayerPrefs.GetInt("P2_HealthPoints");

        Debug.Log("Game Loaded");
    }

    public void NewGame(string firstSceneName)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(firstSceneName);
    }
}
