using UnityEngine;

public class guidePanel : MonoBehaviour
{
    public GameObject guide;
    private GameObject previousPanel;
    public void OpenGuide(GameObject fromPanel)
    {
        previousPanel = fromPanel;
        fromPanel.SetActive(false);
        guide.SetActive(true);
    }

    public void Back()
    {
        guide.SetActive(false);
        if (previousPanel != null)
        {
            previousPanel.SetActive(true);
        }
    }
}
