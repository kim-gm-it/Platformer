using UnityEngine;

public class LoadSavaButton : MonoBehaviour
{
    public void OnSaveButton()
    {
        SaveLoadManager.Instance.SaveGame();
    }

    public void OnLoadButton()
    {
        SaveLoadManager.Instance.LoadGame();
    }
}
