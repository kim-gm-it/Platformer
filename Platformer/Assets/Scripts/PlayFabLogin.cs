using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "17CB36";
        }
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    //private void Start()
    //{
    //    Login();
    //}
    //void Login()
    //{
    //    var request = new LoginWithCustomIDRequest
    //    {
    //        CustomId = SystemInfo.deviceUniqueIdentifier,
    //        CreateAccount = true
    //    };
    //    PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    //}

    //void OnSuccess(LoginResult result)
    //{
    //    Debug.Log("Successful login / account created!");
    //}

    //void OnError(PlayFabError error)
    //{
    //    Debug.Log("Error while logging in/creating account!");
    //    Debug.Log(error.GenerateErrorReport());
    //}
}
