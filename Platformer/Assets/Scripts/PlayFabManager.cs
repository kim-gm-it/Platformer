using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text errorText;

    public void SignUp()
    {
        if (password.text.Length < 6)
        {
            errorText.text = "Password too short!";
            Debug.Log("Password too short");
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignupSuccess, OnError);



    }

    void OnSignupSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Signup successful");
        errorText.text = "Signup successful!";
        SceneManager.LoadScene("Lobby");
    }

    void OnError(PlayFabError error)
    {
        errorText.text = "Error!\n" + error.GenerateErrorReport();
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = password.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }


    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful");
        errorText.text = "Login successful!";
        SceneManager.LoadScene("Lobby");

    }

    public void RecoverPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = "17CB36"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }

    void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        errorText.text = "Recovery email sent to " + email.text ;
        Debug.Log("Recovery email sent");
    }

}

