using UnityEngine;

public class LevelOneFinishPoint : MonoBehaviour
{
    public bool goNextLevel;
    public string levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2") ){
            if (goNextLevel)
            {
                SceneManagement.Instance.nextLevel();
            }
            else
            {
                SceneManagement.Instance.loadScene(levelName);
            }
        }
    }
}
