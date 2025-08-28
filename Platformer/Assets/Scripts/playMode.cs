using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playMode : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject modePanel;
    public void MainMenu()
    {
        // modePanel.SetActive(false);
        SceneManager.LoadScene("mainMenu");
    }
    public void openMenu()
    {
        modePanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    public void ShowPrevScene()
    {
        SceneManager.LoadScene("Login & Signup");
    }
}
