using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class CheckQuesCompleted : MonoBehaviour
{
    public class Flag
    {
        public bool isFinished;
    }

    //Check if the questionnaire is completed
    public IEnumerator CheckQuesStatus()
    {
        //Get the questionnaire status from the server
        IEnumerator getCoroutine = AuthenticationManager.GetQuestionnaireStatus("http://localhost:8080/questions/sendResults");

        yield return StartCoroutine(getCoroutine);
        string responseBody = getCoroutine.Current as string;

        //If the response is not null then set the status of the Questionnaire in the player prefs
        if (responseBody != null)
        {
            //Set the status of the Questionnaire in the player prefs
            Flag Response = JsonUtility.FromJson<Flag>(responseBody);
            bool isFinishedFlag = Response.isFinished;
            if (isFinishedFlag)
            {
                PlayerPrefs.SetString("IsQuestionnaireCompleted", "True");   //Set the status of the Questionnaire in the player prefs
                PlayerPrefs.SetString("InitialScore","False");
                PlayerPrefs.SetString("MissionCompleted", "Level0");
            }
            else
            {
                PlayerPrefs.SetString("IsQuestionnaireCompleted", "False");   //Set the status of the Questionnaire in the player prefs
            }
            yield return true;
        }
        else
        {
            Debug.Log("Error in authentication ");
            yield return false;
        }
    }


}