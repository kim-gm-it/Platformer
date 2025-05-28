using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;
    private GameObject previousPanel;        
    public AudioSource musicSource;
    private bool isMuted = false;
    

    public void OpenSettings(GameObject fromPanel)
    {
        previousPanel = fromPanel;

        fromPanel.SetActive(false);         
        settingsPanel.SetActive(true);      
    }
    public void Back()
    {
        Debug.Log("Back pressed");
        settingsPanel.SetActive(false);     
        if (previousPanel != null)
        {
            previousPanel.SetActive(true);  
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
    }
}
