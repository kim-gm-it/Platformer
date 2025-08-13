using UnityEngine;

public class guidePanel : MonoBehaviour
{
    public GameObject guide;
    public GameObject guide2;
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
    public void level3_guide()
    {
        guide.SetActive(false);
        guide2.SetActive(true);

    }
    public void level3_back()
    {
        guide2.SetActive(false);
        guide.SetActive(true);
    }
}
