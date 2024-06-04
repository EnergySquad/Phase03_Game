using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console_task_bar : MonoBehaviour
{
    public GameObject taskBar; // Reference to the GameObject containing the scrollbar

    private void Start()
    {
        if (taskBar != null)
        {
            taskBar.SetActive(false); // Initially hide the scrollbar
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show the scrollbar when the player enters the trigger zone
            if (taskBar != null)
            {
                taskBar.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the scrollbar when the player exits the trigger zone
            if (taskBar != null)
            {
                taskBar.SetActive(false);
            }
        }
    }
}
