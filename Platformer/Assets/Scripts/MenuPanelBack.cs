using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanelBack : MonoBehaviour
{
    public void ShowPrevScene()
    {
        SceneManager.LoadScene("Login & Signup");
    }
}
