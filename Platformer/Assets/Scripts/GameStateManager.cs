using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject youWinPanel;

    [SerializeField] private AudioSource audioWin;
    [SerializeField] private AudioSource audioLose;

    public bool requireBothPlayersDead = false;

    private bool player1Dead = false;
    private bool player2Dead = false;
    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        youWinPanel.SetActive(false);
    }

    public void PlayerDied(int playerNumber)
    {
        if (gameEnded) return;

        if (playerNumber == 1) player1Dead = true;
        else if (playerNumber == 2) player2Dead = true;

        if (requireBothPlayersDead)
        {
            if (player1Dead && player2Dead)
                TriggerGameOver();
        }
        else
        {
            if (player1Dead || player2Dead)
                TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(ShowGameOverAfterDelay(0.6f));
    }

    IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        if (audioLose != null) audioLose.Play();
    }

    public void ShowYouWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        StartCoroutine(ShowYouWinDelay());
    }

    private IEnumerator ShowYouWinDelay()
    {
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0;
        youWinPanel.SetActive(true);
        if (audioWin != null) audioWin.Play();
    }


    // ---------- BUTTONS ----------
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        youWinPanel.SetActive(false);
        SceneManager.LoadScene("mainMenu");
    }

    public void SaveSceneFromGameOver()
    {
        SaveLoadManager.Instance.SaveSceneOnly();
    }
    public void QuitTheGame()
    {
        Application.Quit();
        Debug.Log("Quit called");
    }
}
