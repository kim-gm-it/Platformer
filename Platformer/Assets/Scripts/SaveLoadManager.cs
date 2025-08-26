using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int p1HealthPoints;
    public float p1HealthBar;
    public float p1PosX, p1PosY, p1PosZ;

    public int p2HealthPoints;
    public float p2HealthBar;
    public float p2PosX, p2PosY, p2PosZ;

    public int bossHealth;
    public float bossPosX, bossPosY, bossPosZ;
}

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    [Header("Player References")]
    public BasePlayer player1;
    public BasePlayer player2;

    [Header("Boss Reference (Level 3)")]
    public Boss boss;

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

    // ---------- SAVE ----------
    public void SaveSceneOnly()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("SceneOnly", 1); 
        PlayerPrefs.Save();

        SaveToCloud();
        Debug.Log("Scene-only saved!");
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("SceneOnly", 0);

        if (player1 == null)
            player1 = GameObject.FindWithTag("Player1")?.GetComponent<BasePlayer>();
        if (player2 == null)
            player2 = GameObject.FindWithTag("Player2")?.GetComponent<BasePlayer>();
        if (boss == null)
            boss = FindFirstObjectByType<Boss>();

        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);

        // Player 1
        if (player1 != null)
        {
            PlayerPrefs.SetInt("P1_IsAlive", player1.livesPoint > 0 ? 1 : 0);
            PlayerPrefs.SetFloat("P1_HealthBar", player1.livesBar);
            PlayerPrefs.SetInt("P1_HealthPoints", player1.livesPoint);
            PlayerPrefs.SetFloat("P1_PosX", player1.transform.position.x);
            PlayerPrefs.SetFloat("P1_PosY", player1.transform.position.y);
            PlayerPrefs.SetFloat("P1_PosZ", player1.transform.position.z);
        }

        // Player 2
        if (player2 != null)
        {
            PlayerPrefs.SetInt("P2_IsAlive", player2.livesPoint > 0 ? 1 : 0);
            PlayerPrefs.SetFloat("P2_HealthBar", player2.livesBar);
            PlayerPrefs.SetInt("P2_HealthPoints", player2.livesPoint);
            PlayerPrefs.SetFloat("P2_PosX", player2.transform.position.x);
            PlayerPrefs.SetFloat("P2_PosY", player2.transform.position.y);
            PlayerPrefs.SetFloat("P2_PosZ", player2.transform.position.z);
        }

        // Boss
        if (boss != null)
        {
            PlayerPrefs.SetInt("Boss_Health", boss.IsDead() ? 0 : boss.currentHealth);
            PlayerPrefs.SetFloat("Boss_PosX", boss.transform.position.x);
            PlayerPrefs.SetFloat("Boss_PosY", boss.transform.position.y);
            PlayerPrefs.SetFloat("Boss_PosZ", boss.transform.position.z);
        }

        PlayerPrefs.Save();

        SaveToCloud(); 
        Debug.Log("Full game saved!");
    }

    // ---------- CLOUD SAVE ----------
    public void SaveToCloud()
    {
        SaveData data = new SaveData
        {
            p1HealthPoints = PlayerPrefs.GetInt("P1_HealthPoints", 0),
            p1HealthBar = PlayerPrefs.GetFloat("P1_HealthBar", 0),
            p1PosX = PlayerPrefs.GetFloat("P1_PosX", 0),
            p1PosY = PlayerPrefs.GetFloat("P1_PosY", 0),
            p1PosZ = PlayerPrefs.GetFloat("P1_PosZ", 0),

            p2HealthPoints = PlayerPrefs.GetInt("P2_HealthPoints", 0),
            p2HealthBar = PlayerPrefs.GetFloat("P2_HealthBar", 0),
            p2PosX = PlayerPrefs.GetFloat("P2_PosX", 0),
            p2PosY = PlayerPrefs.GetFloat("P2_PosY", 0),
            p2PosZ = PlayerPrefs.GetFloat("P2_PosZ", 0),

            bossHealth = PlayerPrefs.GetInt("Boss_Health", 0),
            bossPosX = PlayerPrefs.GetFloat("Boss_PosX", 0),
            bossPosY = PlayerPrefs.GetFloat("Boss_PosY", 0),
            bossPosZ = PlayerPrefs.GetFloat("Boss_PosZ", 0),
        };

        string json = JsonUtility.ToJson(data);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "SaveFile", json }
            }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log(" Cloud Save successful!"),
            error => Debug.LogError(" Cloud Save failed: " + error.GenerateErrorReport()));
    }

    public void LoadFromCloud(System.Action callback = null)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                if (result.Data != null && result.Data.ContainsKey("SaveFile"))
                {
                    string json = result.Data["SaveFile"].Value;
                    SaveData data = JsonUtility.FromJson<SaveData>(json);

                    PlayerPrefs.SetInt("P1_HealthPoints", data.p1HealthPoints);
                    PlayerPrefs.SetFloat("P1_HealthBar", data.p1HealthBar);
                    PlayerPrefs.SetFloat("P1_PosX", data.p1PosX);
                    PlayerPrefs.SetFloat("P1_PosY", data.p1PosY);
                    PlayerPrefs.SetFloat("P1_PosZ", data.p1PosZ);

                    PlayerPrefs.SetInt("P2_HealthPoints", data.p2HealthPoints);
                    PlayerPrefs.SetFloat("P2_HealthBar", data.p2HealthBar);
                    PlayerPrefs.SetFloat("P2_PosX", data.p2PosX);
                    PlayerPrefs.SetFloat("P2_PosY", data.p2PosY);
                    PlayerPrefs.SetFloat("P2_PosZ", data.p2PosZ);

                    PlayerPrefs.SetInt("Boss_Health", data.bossHealth);
                    PlayerPrefs.SetFloat("Boss_PosX", data.bossPosX);
                    PlayerPrefs.SetFloat("Boss_PosY", data.bossPosY);
                    PlayerPrefs.SetFloat("Boss_PosZ", data.bossPosZ);

                    PlayerPrefs.Save();
                    Debug.Log(" Cloud Load successful!");
                }
                else
                {
                    Debug.Log(" No cloud save found.");
                }

                callback?.Invoke();
            },
            error =>
            {
                Debug.LogError(" Cloud Load failed: " + error.GenerateErrorReport());
                callback?.Invoke();
            });
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavedScene"))
        {
            Debug.LogWarning("No saved game found!");
            return;
        }

        string sceneToLoad = PlayerPrefs.GetString("SavedScene");
        bool sceneOnly = PlayerPrefs.GetInt("SceneOnly", 0) == 1;

        if (sceneOnly)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        player1 = GameObject.FindWithTag("Player1")?.GetComponent<BasePlayer>();
        player2 = GameObject.FindWithTag("Player2")?.GetComponent<BasePlayer>();
        boss = FindFirstObjectByType<Boss>();

        // Player 1
        if (player1 != null)
        {
            player1.livesBar = PlayerPrefs.GetFloat("P1_HealthBar", 0);
            player1.livesPoint = PlayerPrefs.GetInt("P1_HealthPoints", 0);
            player1.transform.position = new Vector3(
                PlayerPrefs.GetFloat("P1_PosX", 0),
                PlayerPrefs.GetFloat("P1_PosY", 0),
                PlayerPrefs.GetFloat("P1_PosZ", 0)
            );
            player1.RefreshUI();

            if (PlayerPrefs.GetInt("P1_IsAlive", 1) == 0)
            {
                Destroy(player1.gameObject);
                player1 = null;
            }
        }

        // Player 2
        if (player2 != null)
        {
            player2.livesBar = PlayerPrefs.GetFloat("P2_HealthBar", 0);
            player2.livesPoint = PlayerPrefs.GetInt("P2_HealthPoints", 0);
            player2.transform.position = new Vector3(
                PlayerPrefs.GetFloat("P2_PosX", 0),
                PlayerPrefs.GetFloat("P2_PosY", 0),
                PlayerPrefs.GetFloat("P2_PosZ", 0)
            );
            player2.RefreshUI();

            if (PlayerPrefs.GetInt("P2_IsAlive", 1) == 0)
            {
                Destroy(player2.gameObject);
                player2 = null;
            }
        }

        // Boss
        if (boss != null)
        {
            boss.currentHealth = PlayerPrefs.GetInt("Boss_Health", boss.maxHealth);
            boss.transform.position = new Vector3(
                PlayerPrefs.GetFloat("Boss_PosX", boss.transform.position.x),
                PlayerPrefs.GetFloat("Boss_PosY", boss.transform.position.y),
                PlayerPrefs.GetFloat("Boss_PosZ", boss.transform.position.z)
            );
            boss.UpdateHealthBar(boss.currentHealth, boss.maxHealth);
        }

        Debug.Log("Full game loaded!");
    }

    // ---------- NEW GAME ----------
    public void NewGame(string firstSceneName)
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(firstSceneName);
    }
}