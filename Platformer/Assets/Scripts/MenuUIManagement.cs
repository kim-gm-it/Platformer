using UnityEngine;


public class MenuUIManagement : MonoBehaviour
{
    [SerializeField] private GameObject connectingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject lobbiesPanel;
    //[SerializeField] private GameObject characterSelectionPanel;

    public void ShowConnecting()
    {
        connectingPanel.SetActive(true);
        menuPanel.SetActive(false);
        lobbiesPanel.SetActive(false);
        //characterSelectionPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        connectingPanel.SetActive(false);
        menuPanel.SetActive(true);
        lobbiesPanel.SetActive(false); 
        //characterSelectionPanel.SetActive(false);
    }

    public void ShowLobbies()
    {
        connectingPanel.SetActive(false);
        menuPanel.SetActive(false);
        lobbiesPanel.SetActive(true);
        //characterSelectionPanel.SetActive(false);
    }
    
    public void ShowCharacterSelection()
    {
        connectingPanel.SetActive(false);
        menuPanel.SetActive(false);
        lobbiesPanel.SetActive(false);
        //characterSelectionPanel.SetActive(true);
    }
}
