using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DisplayPDetails")
        {
            // Find the GameObject with DisplayText.cs script attached
            GameObject displayTextObject = GameObject.Find("DisplayPlayerDetails");

            if (displayTextObject != null)
            {
                // Get the DisplayText component and call DisplayPlayerDetails()
                DisplayText displayText = displayTextObject.GetComponent<DisplayText>();
                if (displayText != null)
                {
                    displayText.DisplayPlayerDetails();
                }
                else
                {
                    Debug.LogError("DisplayText component not found on GameObject: " + displayTextObject.name);
                }
            }
            else
            {
                Debug.LogError("GameObject with name 'DisplayPlayerDetails' not found in the scene.");
            }
        }
    }
}
