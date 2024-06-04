using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Load the next scene
    public void LoadNextScene(string gamingSceneName)
    {
        if (!string.IsNullOrEmpty(gamingSceneName))
        {
            SceneManager.LoadScene(gamingSceneName);
        }
        else
        {
            Debug.LogError("Gaming scene name is not specified!");
        }
    }

    // Load the player profile page
    public void LoadProfilePage()
    {
        LoadNextScene("DisplayPDetails");
    }

    // Load the questionnaire page
    public void LoadQuestionnairePage()
    {
        LoadNextScene("Questionnaire");
    }

    // Load the game page
    public void LoadGame()
    {
        LoadNextScene("testScene");
    }

    // Load the welcome page
    public void LoadWelcomeWindow()
    {
        LoadNextScene("WelcomePage");
    }
}