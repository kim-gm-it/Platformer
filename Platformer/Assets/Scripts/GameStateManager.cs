using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public GameObject startPanel;
    public AudioSource audioWin;
    public AudioSource audioLose;

    void Start()
    {
        // Get the AudioSource components attached to this GameObject
        audioWin=GetComponent<AudioSource>();
        audioLose=GetComponent<AudioSource>();
    }

     // This function is called to display the Game Over panel and play the lose sound
    public void ShowGameOver()
    {
        // Stop time in the game to freeze the current state
        Time.timeScale = 0;
        // Activate the Game Over panel in the UI
        gameOverPanel.SetActive(true);
        // Play the lose sound effect
        audioLose.Play();
    }
    public void RestartGame(){
        // Ensure time is running again before restarting
        Time.timeScale = 1; 
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    // This function is called to display the You Win panel and play the win sound
    public void ShowYouWin()
    {
        // Stop time in the game to freeze the current state
        Time.timeScale = 0;
        // Activate the You Win panel in the UI
        youWinPanel.SetActive(true);
        // Play the win sound effect
        audioWin.Play();

    }
}
