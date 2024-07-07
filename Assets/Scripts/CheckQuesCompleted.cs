using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

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
                if (PlayerPrefs.GetInt("InitialScore") == -1)
                {
                    PlayerPrefs.SetInt("InitialScore", 0);
                    PlayerPrefs.SetString("MissionCompleted", "Level0");
                    yield return StartCoroutine(GetOverallIlluminance());
                }
                
            }
            else
            {
                PlayerPrefs.SetInt("InitialScore", -1);
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

    IEnumerator GetOverallIlluminance()
    {
        // URL of the endpoint to fetch overall illuminance
        string url = "http://localhost:8080/questions/sendResults";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the GET request
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(jsonResponse);
                PlayerPrefs.SetFloat("TotalScore", response.score * 1000);
            }
            else
            {
                Debug.LogError("Failed to fetch overall illuminance: " + webRequest.error);
            }
        }
    }

}