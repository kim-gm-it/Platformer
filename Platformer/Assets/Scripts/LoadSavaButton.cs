using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSavaButton : MonoBehaviour
{
    public GameObject load;
    public GameObject StaarPanel;
     public GameObject errorText;
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
        try
        {
            SaveLoadManager.Instance.LoadFromCloud(() =>
            {
                SaveLoadManager.Instance.LoadGame();
            });
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to load the game: " + e.Message);
            if (errorText != null)
            {
                errorText.SetActive(true); 
                Invoke("HideErrorText", 3f); 
            }
        }
    }

    private void HideErrorText()
    {
        if(errorText != null)
            errorText.SetActive(false);
    }
    public void StartTheGame()
    {
        Time.timeScale = 1;
        // SceneManager.LoadScene("level1");
        SaveLoadManager.Instance.NewGame("level1");
    }
    public void Back()
    {
        load.SetActive(false);
        StaarPanel.SetActive(true);
        
    }
}
