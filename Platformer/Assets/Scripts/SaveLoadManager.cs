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
        if (player1 == null)
            player1 = GameObject.FindWithTag("Player1")?.GetComponent<BasePlayer>();
        if (player2 == null)
            player2 = GameObject.FindWithTag("Player2")?.GetComponent<BasePlayer>();

        if (player1 == null || player2 == null)
        {
            Debug.LogError("Players not found! Cannot save.");
            return;
        }

        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);

        PlayerPrefs.SetFloat("P1_HealthBar", player1.livesBar);
        PlayerPrefs.SetInt("P1_HealthPoints", player1.livesPoint);

        PlayerPrefs.SetFloat("P2_HealthBar", player2.livesBar);
        PlayerPrefs.SetInt("P2_HealthPoints", player2.livesPoint);

        PlayerPrefs.SetFloat("P1_PosX", player1.transform.position.x);
        PlayerPrefs.SetFloat("P1_PosY", player1.transform.position.y);
        PlayerPrefs.SetFloat("P1_PosZ", player1.transform.position.z);

        PlayerPrefs.SetFloat("P2_PosX", player2.transform.position.x);
        PlayerPrefs.SetFloat("P2_PosY", player2.transform.position.y);
        PlayerPrefs.SetFloat("P2_PosZ", player2.transform.position.z);

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

        Vector3 pos1 = new Vector3(
            PlayerPrefs.GetFloat("P1_PosX"),
            PlayerPrefs.GetFloat("P1_PosY"),
            PlayerPrefs.GetFloat("P1_PosZ")
        );
        player1.transform.position = pos1;

        Vector3 pos2 = new Vector3(
            PlayerPrefs.GetFloat("P2_PosX"),
            PlayerPrefs.GetFloat("P2_PosY"),
            PlayerPrefs.GetFloat("P2_PosZ")
        );

        player2.transform.position = pos2;
        player1.livesBar = PlayerPrefs.GetFloat("P1_HealthBar");
        player1.livesPoint = PlayerPrefs.GetInt("P1_HealthPoints");
        player1.RefreshUI();

        player2.livesBar = PlayerPrefs.GetFloat("P2_HealthBar");
        player2.livesPoint = PlayerPrefs.GetInt("P2_HealthPoints");
        player2.RefreshUI();

        Debug.Log("Game Loaded");
    }

    public void NewGame(string firstSceneName)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(firstSceneName);
    }
}
