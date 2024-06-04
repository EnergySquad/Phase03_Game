using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public GameObject[] task_1;
    public GameObject[] task_2;
    public GameObject[] task_3;
    public GameObject[] task_4;
    public GameObject[] task_5_1;
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    public GameObject door5;
    public GameObject scoreCalculator;

    private bool isLevel1Updated;
    private bool isLevel2Updated;
    private bool isLevel3Updated;
    private bool isLevel4Updated;
    private bool isLevel5Updated;


    void Start()
    {
        // Initialize PlayerPrefs
        PlayerPrefs.SetString("MissionCompleted", "Level4");
        PlayerPrefs.SetFloat("TotalScore",750f);
        PlayerPrefs.SetString("IdeleState", "true");

        // Initialize doors and tasks
        InitializeDoors();
        InitializeTasks();
        InitializeLevels();


    }
    // Update is called once per frame
    void Update()
    {
        switch (PlayerPrefs.GetString("IdeleState"))
        {
            case "true":
                // Handle transitions when in idle state
                switch (PlayerPrefs.GetString("MissionCompleted"))
                {
                    case "Level0":
                        door1.SetActive(true);
                        break;
                    case "Level1":
                        door1.SetActive(false);
                        door2.SetActive(true);
                        break;
                    case "Level2":
                        door2.SetActive(false);
                        door3.SetActive(true);
                        break;
                    case "Level3":
                        door3.SetActive(false);
                        door4.SetActive(true);
                        break;
                    case "Level4":
                        door4.SetActive(false);
                        door5.SetActive(true);
                        break;
                    case "Level5":
                        door5.SetActive(false);
                        Debug.Log("Task 5 is completed");
                        Debug.Log("All tasks are completed");
                        SceneManager.LoadScene("WelcomePage");
                        break;
                }
                break;
            case "false":
                // Handle transitions when tasks are active
                switch (PlayerPrefs.GetString("MissionCompleted"))
                {
                    case "Level0":
                        if (!isLevel1Updated)
                        {
                            scoreCalculator.GetComponent<ScoreCalculator>().SetDecreasingConst(10.0f);
                            SetActiveState(task_1, true);
                            Debug.Log("Task 1 is active");
                            isLevel1Updated = true;
                        }
                        break;
                    case "Level1":
                        if (!isLevel2Updated)
                        {
                            scoreCalculator.GetComponent<ScoreCalculator>().SetDecreasingConst(10.0f);
                            SetActiveState(task_2, true);
                            Debug.Log("Task 2 is active");
                            isLevel2Updated = true;
                        }
                        break;
                    case "Level2":
                        if (!isLevel3Updated)
                        {
                            scoreCalculator.GetComponent<ScoreCalculator>().SetDecreasingConst(10.0f);
                            SetActiveState(task_3, true);
                            Debug.Log("Task 3 is active");
                            isLevel3Updated = true;
                        }
                        break;
                    case "Level3":
                        if (!isLevel4Updated)
                        {
                            scoreCalculator.GetComponent<ScoreCalculator>().SetDecreasingConst(10.0f);
                            SetActiveState(task_4, true);
                            Debug.Log("Task 5 is active");
                            isLevel4Updated = true;
                        }
                        break;
                    case "Level4":
                        if (!isLevel5Updated)
                        {
                            scoreCalculator.GetComponent<ScoreCalculator>().SetDecreasingConst(10.0f);
                            SetActiveState(task_5_1, true);
                            Debug.Log("Task 5 is active");
                            isLevel5Updated = true;
                        }
                        break;
                }
                break;
        }
    }

    private void SetActiveState(GameObject[] gameObjects, bool state)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(state);
        }
    }

    private void InitializeLevels()
    {
        isLevel1Updated = false;
        isLevel2Updated = false;
        isLevel3Updated = false;
        isLevel4Updated = false;
        isLevel5Updated = false;
    }

    private void InitializeDoors()
    {
        door1.SetActive(false);
        door2.SetActive(false);
        door3.SetActive(false);
        door4.SetActive(false);
        door5.SetActive(false);
    }

    private void InitializeTasks()
    {
        SetActiveState(task_1, false);
        SetActiveState(task_2, false);
        SetActiveState(task_3, false);
        SetActiveState(task_4, false);
        SetActiveState(task_5_1, false);
    }
}