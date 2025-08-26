using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionBackBTN : MonoBehaviour
{
    public void ShowMenuPanel()
    {
        GameObject.Find("PanelManager").GetComponent<MenuUIManagement>().ShowMenu();
    }
}
