using UnityEngine;

public class LevelOneFinishPoint : MonoBehaviour
{
    public bool goNextLevel;
    public string levelName = "level2";

    public AudioClip finishSound; 
    private AudioSource audioSource;

    private bool hasTriggered = false; 
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            hasTriggered = true;

            if (finishSound != null)
            {
                audioSource.PlayOneShot(finishSound);
            }

            Invoke(nameof(LoadNextLevel), 1f);
        }
    }

    private void LoadNextLevel()
    {
        if (goNextLevel)
        {
            Debug.Log("Going to scene: " + levelName);
            SceneManagement.Instance.nextLevel();
        }
        else
        {
            SceneManagement.Instance.loadScene(levelName);
        }
    }
}
