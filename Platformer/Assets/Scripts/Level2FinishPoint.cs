using UnityEngine;

public class Level2FinishPoint : MonoBehaviour
{
    public bool goNextLevel;
    public string levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if (collision.gameObject.GetComponent<Player1Controller>().hasKey)
            {
                SceneManagement.Instance.loadScene(levelName);
            }

        }else if (collision.gameObject.CompareTag("Player2"))
        {
            if (collision.gameObject.GetComponent<Player2Controler>().hasKey)
            {
                SceneManagement.Instance.loadScene(levelName);
            }
        }
    }
}
