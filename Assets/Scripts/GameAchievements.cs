using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameAchievements : MonoBehaviour
{
    public Text results;
    public PopUpMessage popUpScore;
    private string Achievement;

    public class Score
    {
        public bool isFinished;
        public float score;
    }

    public void GetAchievements()
    {
        StartCoroutine(getAchievements());
    }

    public IEnumerator getAchievements()
    {
        // Load the Achievements scene
        IEnumerator getCoroutine = AuthenticationManager.GetQuestionnaireStatus("http://localhost:8080/questions/sendResults");
        yield return StartCoroutine(getCoroutine);
        string responseBody = getCoroutine.Current as string;

        if (responseBody != null)
        {
            Score Response = JsonUtility.FromJson<Score>(responseBody);
            Debug.Log("Response=" + Response);
            float Score = Response.score;
            if (Score > 0.7)
            {
                popUpScore.GetComponent<PopUpMessage>().ClickButton();
                Achievement = "Early Striker";
                results.text = Achievement;
            }
        }
        else
        {
            Debug.Log("Error in authentication ");
        }
    }
}
