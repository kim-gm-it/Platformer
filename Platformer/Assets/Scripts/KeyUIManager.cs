using UnityEngine;

public class KeyUIManager : MonoBehaviour
{
    public static KeyUIManager instance;

    [SerializeField] private GameObject keyIcon;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void ShowKeyIcon()
    {
        if (keyIcon != null)
        {
            keyIcon.SetActive(true);
        }
    }
}
