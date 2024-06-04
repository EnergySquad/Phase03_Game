using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void ExitTheGame()
    {
        QuitGame();
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            // UnityEditor.EditorApplication.isPlaying = false;     // If the game is running in the Unity Editor, stop playing the scene.
        }
        else
        {
            Application.Quit(); // Quit the game if it is running in a standalone build.
        }
    }

    public void ReturnMenu () {
        SceneManager.LoadScene("WelcomePage");
    }
}
