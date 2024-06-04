using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private const string apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmM4OjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJiZQ";
    private const string baseUrl = "http://20.15.114.131:8080/api/login";
    public SceneLoader sceneLoader;
    public NavigationCommands backToWelcomePage;
    private string currentSceneName;

    public static Login Instance { get; private set; }
    public static string TokenKey { get; private set; } = "JWTToken";

    private void Awake()
    {
        // Ensure there's only one instance of Login
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AuthenticatePlayer()
    {
        StartCoroutine(AuthenticatePlayerCoroutine());
    }


    private IEnumerator AuthenticatePlayerCoroutine()
    {
        string requestBody = "{\"apiKey\": \"" + apiKey + "\"}";

        //Call the Authenticate method from AuthenticationManager
        IEnumerator authCoroutine = AuthenticationManager.Authenticate(baseUrl, requestBody);
        yield return StartCoroutine(authCoroutine);
        string jsonResponse = authCoroutine.Current as string;

        if (jsonResponse != null)
        {
            // Save the token in PlayerPrefs
            AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(jsonResponse);
            string token = authResponse.token;
            PlayerPrefs.SetString(TokenKey, token);

            currentSceneName = SceneManager.GetActiveScene().name;
            // Load the Next scene
            backToWelcomePage.GetComponent<NavigationCommands>().GoToWelcomePage(currentSceneName);
        }
        else
        {
            Debug.LogError("Failed to authenticate player!");
        }

    }


    [System.Serializable]
    public class AuthResponse
    {
        public string token;
    }
}
