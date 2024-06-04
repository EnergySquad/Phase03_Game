using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanelTask : MonoBehaviour
{
    public ScoreCalculator ScoreCalculator;
    public GameObject gameOver;
    private int NoOfAssets = 0;

    //Calculate Score when player collects coins
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            ScoreCalculator.GetComponent<ScoreCalculator>().ScoreIncrement(100);
        }
        if (other.gameObject.CompareTag("SolarTag"))
        {
            Destroy(other.gameObject);
            string OpenObject = other.gameObject.name + "*";
            GameObject panel = GameObject.Find(OpenObject);
            Animator anim = panel.GetComponent<Animator>();
            //Rotate the Solar Panel
            anim.SetTrigger("SolarUp");
            ScoreCalculator.GetComponent<ScoreCalculator>().UpdateDecreasingConst(1.2f);
            NoOfAssets += 1;
        }
        if (NoOfAssets == 4)
        {
            if (PlayerPrefs.GetString("MissionCompleted") == "Level0")
            {
                //update the is_task_1_completed to true
                gameOver.SetActive(true);
                // Pause the game
                Time.timeScale = 0;
                // Wait for the specified amount of real time
                yield return new WaitForSecondsRealtime(2);
                // Resume the game
                Time.timeScale = 1;
                gameOver.SetActive(false);
                // Set the mission completed to Level1 and active the idle state until the player reach to next task
                PlayerPrefs.SetString("MissionCompleted", "Level1");
                PlayerPrefs.SetString("IdeleState", "true");
            }
        }
    }
}
