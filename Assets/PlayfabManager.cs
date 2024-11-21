using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;

public class PlayfabManager 
{
    private LoginManager loginManager;
    private string savedEmailKey = "SavedEmail";
    private string userEmail;

    private void Start()
    {
        loginManager = new LoginManager();
        // Check if email is saved
        if (PlayerPrefs.HasKey(savedEmailKey))
        {
            string savedEmail = PlayerPrefs.GetString(savedEmailKey);
            // Auto login wth saved email
            EmailLoginButtonClicked(savedEmail, "SavedPassword");
        }
    }
    // Example method for triggering email login
    public void EmailLoginButtonClicked(string email, string password)
    {
        userEmail = email;
        loginManager.SetLoginMethod(new EmailLogin(email, password));
        loginManager.Login(OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        // You can handle success here, such as loading player data
        // Save email for future auto-login
        if (!string.IsNullOrEmpty(userEmail))
            PlayerPrefs.SetString(savedEmailKey, userEmail);
        // LoadPlayerData(result.PlayFabId);      
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login faile: " + error.ErrorMessage);
    }
    private void LoadPlayerData(string playFabId)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playFabId
        };
        PlayFabClientAPI.GetUserData(request, OnDataSuccess, OnDataFailure);
    }
    private void OnDataSuccess(GetUserDataRequest result)
    {
        // Process player data here
        Debug.Log("Player data loaded successfully");
    }
    private void OnDataFailure(PlayFabError error)
    {
        Debug.LogError("Failed to load player data: " + error.ErrorMessage);
    }
}
