using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel_MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel1; 
    public GameObject settingPanel;
    public GameObject guidePanel;

    [Header("Scripts")]
    public SettingsPanel settingsPanelScript;
    public guidePanel guidePanelScript;

    public void StartTheGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("level1");
    }

    // Settings
    public void OpenSettings()
    {
        startPanel1.SetActive(false);
        settingPanel.SetActive(true);
        // settingsPanelScript.OpenSettings(startPanel1);
    }

    // Guide
    public void OpenGuide()
    {
        startPanel1.SetActive(false);
        guidePanelScript.OpenGuide(startPanel1);
    }

    // Quit
    public void QuitTheGame()
    {
        Application.Quit();
        Debug.Log("Quit called");
    }
}
