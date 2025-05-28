using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PausePanel : MonoBehaviour
{
    private bool isPaused;
    public GameObject pausePanel;
    public GameObject startPanel;
    public GameObject settingPanel;
    public SettingsPanel settingsPanelScript; 
    public void OpenSettings()
    {
        settingsPanelScript.OpenSettings(pausePanel);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause")){
            if(isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }
    // Pauses the game
    public void PauseGame(){
        // Freeze all game activity
        Time.timeScale=0;
        // Show the pause UI panel
        pausePanel.SetActive(true);
        isPaused=true;
    }
    // Resumes the game from pause
    public void ResumeGame(){
        // Resume time
        Time.timeScale=1;
        // Hide the pause UI panel
        pausePanel.SetActive(false);
        isPaused=false;
    }
    // Restarts the current scene
    public void RestartGame(){
        // Ensure time is running again before restarting
        Time.timeScale = 1; 
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(false);
        startPanel.SetActive(true);
    }
    public void Setting()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(false);
        settingsPanelScript.OpenSettings(pausePanel);

    }
}

