using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public GameObject startPanel1;
    public GameObject settingPanel;
    public GameObject guidePanel;
    public SettingsPanel settingsPanelScript;
    public guidePanel guidePanelScript;
    public void OpenSettings()
    {
        settingsPanelScript.OpenSettings(startPanel1);
    }
    public void OpenGuide()
    {
        guidePanelScript.OpenGuide(startPanel1);
    }

    // Called when Start button is clicked
    void Start()
    {
        // Pause the game when it starts so the player can see the start panel
        Time.timeScale = 0;
    }
    public void StartTheGame()
    {
        Time.timeScale = 1;
        startPanel1.SetActive(false);
    }

    // Called when Quit button is clicked
    public void QuitTheGame()
    {
        Application.Quit();
        Debug.Log("Quit called");
    }
    public void Setting()
    {
        Time.timeScale = 0;
        startPanel1.SetActive(false);
        settingsPanelScript.OpenSettings(startPanel1);
    }
    public void Guide()
    {
        Time.timeScale = 0;
        startPanel1.SetActive(false);
        guidePanelScript.OpenGuide(startPanel1);
    }
}
