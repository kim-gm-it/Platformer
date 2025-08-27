using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionBackBTN : MonoBehaviour
{
    public void ShowMenuPanel()
    {
        GameObject.Find("PanelsManager").GetComponent<MenuUIManagement>().ShowMenu();
    }

    public void ShowMainMenue()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
