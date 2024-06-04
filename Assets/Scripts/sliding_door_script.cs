using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliding_door_script : MonoBehaviour
{
    public Animator MyAnime;
    public string Player;
    public string OpenCloseAnimeBoolName;
    public GameObject instructions;
    public string PrevTaskLevel;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Player)
        {
            MyAnime.SetBool(OpenCloseAnimeBoolName, true); // Set the bool value to true to open the door
            Debug.Log(PrevTaskLevel);
            if (PlayerPrefs.GetString("MissionCompleted") == PrevTaskLevel)     // Check if the previous task is completed
            {
                instructions.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.Confined;  // Lock the cursor to the game window
                Cursor.visible = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Player)
        {
            MyAnime.SetBool(OpenCloseAnimeBoolName, false);
        }
    }

    public void Resume_Game()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
        Cursor.visible = true;
        instructions.SetActive(false);
        PlayerPrefs.SetString("IdeleState", "false");
    }
}
