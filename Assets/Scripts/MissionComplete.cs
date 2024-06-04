using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChildChecker : MonoBehaviour
{
    public GameObject gameOver;

    void Update()
    {
        // Check if the GameObject has any child objects
        if (transform.childCount == 0)
        {
            PlayerPrefs.SetString("MissionCompleted", "Level5");
            Debug.Log("Mission Completed");
        }

        if (PlayerPrefs.GetString("MissionCompleted") == "Level5")
        {
            StartCoroutine(HandleGameOver());
        }
    }

    IEnumerator HandleGameOver()
    {
        // Update the is_task_1_completed to true
        // gameOver.SetActive(true);
        // Pause the game
        Time.timeScale = 0;
        // Wait for the specified amount of real time
        yield return new WaitForSecondsRealtime(2);
        // Resume the game
        Time.timeScale = 1;
        // gameOver.SetActive(false);
        // Set the mission completed to Level1 and activate the idle state until the player reaches the next task
        PlayerPrefs.SetString("MissionCompleted", "Level5");
        PlayerPrefs.SetString("IdeleState", "true");
        SceneManager.LoadScene("Mission Completed");
    }
}
