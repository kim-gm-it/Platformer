using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalMusicManager : MonoBehaviour
{
    public AudioSource globalMusic; 
    public string[] scenesWithOwnMusic;

    private static GlobalMusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isSpecialScene = false;
        foreach (string sceneName in scenesWithOwnMusic)
        {
            if (scene.name == sceneName)
            {
                isSpecialScene = true;
                break;
            }
        }

        if (isSpecialScene)
        {
            if (globalMusic.isPlaying)
                globalMusic.Stop();
        }
        else
        {
            if (!globalMusic.isPlaying)
                globalMusic.Play();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
