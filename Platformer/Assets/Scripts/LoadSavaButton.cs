using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSavaButton : MonoBehaviour
{
    public void OnSaveButton()
    {
        SaveLoadManager.Instance.SaveGame();
    }

    public void OnLoadButton()
    {
        SaveLoadManager.Instance.LoadGame();
    }
    public void StartTheGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("level1");
    }
}
