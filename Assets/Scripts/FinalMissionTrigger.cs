using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMissionTrigger : MonoBehaviour
{
    public GameObject instructions;
    private bool isTriggerred = false;

    private void OnTriggerEnter(Collider other)
    { 
        if(!isTriggerred) {
        if (other.tag == "Player")
        {
            instructions.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;  // Lock the cursor to the game window
            Cursor.visible = true;
            isTriggerred = true;
            // Destroy(gameObject);
        }
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
