using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelection : MonoBehaviour
{

    public GameObject[] characters;
    public int selectedCharacter = 0;
    public int playerId = 1;
    

    public void NextCharacter()
    {

        Debug.Log("Next character button pressed");
        characters[selectedCharacter].gameObject.SetActive(false);
        selectedCharacter = (selectedCharacter + 1)%characters.Length;
        characters[selectedCharacter].gameObject.SetActive(true);   
    }

    public void PrevCharacter()
    {
        Debug.Log("Prev character button pressed");
        characters[selectedCharacter].SetActive(false);

        selectedCharacter--;
        
        if(selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].gameObject.SetActive(true);
    }

    public void Next()
    {

        Debug.Log("Next button pressed");
        
        //save the selected character , with a player number in the key
        PlayerPrefs.SetInt($"P{playerId}_SelectedCharacter", selectedCharacter);
        
        //mark player as ready
        PlayerPrefs.SetInt($"P{playerId}_Ready", 1);

        Debug.Log($"Player {playerId} ready with character {selectedCharacter}");

        if(PlayerPrefs.GetInt("P1_Ready" , 0) == 1 && PlayerPrefs.GetInt("P2_Ready" , 0) == 1)
        {
            Debug.Log("both players ready");
            SceneManager.LoadScene("mainMenu");
        }
        
    }
}
