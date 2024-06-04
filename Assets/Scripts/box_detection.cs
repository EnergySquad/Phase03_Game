using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Assuming you are using UI Canvas

public class BoxDetection : MonoBehaviour
{
    public GameObject completed;
   
    public ScoreCalculator ScoreCalculator;
    public GameObject box;
    // public GameObject task_finished;
    // public ColiderCalculator ColiderCalculator;

    void Start()
    {
        // trial = ColiderCalculator.trial;
        completed.SetActive(false);
    }

    // if the floor is triggered by the rigid body of the box set completed as true
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == box)
        {
            completed.SetActive(true);
            // trial +=1;
            // Debug.Log("Trial is " + trial);
            // ColiderCalculator.setTrial(trial); 
            ScoreCalculator.GetComponent<ScoreCalculator>().UpdateDecreasingConst(5.0f);
            StartCoroutine(Completed());

          
            // Debug.Log("Trial 2 are completed");
            PlayerPrefs.SetString("MissionCompleted", "Level4");
            PlayerPrefs.SetString("IdeleState", "true");
        }
        
    }

    IEnumerator Completed()
    {
        yield return new WaitForSeconds(3);
        completed.SetActive(false);
    }


    // IEnumerator TaskFinished()
    // {
    //     yield return new WaitForSeconds(3);
    //     task_finished.SetActive(false);
    // }

       
   
}
