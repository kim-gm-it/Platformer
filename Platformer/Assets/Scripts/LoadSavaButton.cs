using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSavaButton : MonoBehaviour
{
    public GameObject load;
    public GameObject StaarPanel;
    public void OnSaveButton()
    {
        SaveLoadManager.Instance.SaveGame();
    }
    
    public void OnSaveSceneButton()
    {
        SaveLoadManager.Instance.SaveSceneOnly();
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
    public void Back()
    {
        load.SetActive(false);
        StaarPanel.SetActive(true);
        
    }
}
