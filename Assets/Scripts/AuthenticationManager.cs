//POST, GRT, PUT requests for authentication, get questionnaire status

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public static class AuthenticationManager
{
    //Authenticate user
    public static IEnumerator Authenticate(string baseUrl, string requestBody)
    {
        var request = new UnityWebRequest(baseUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            yield return jsonResponse;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    //Get Request
    public static IEnumerator GetProfile(string url, string jwtToken)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                string responseBody = request.downloadHandler.text;
                yield return responseBody;
            }
        }
        else
        {
            Debug.LogError("Error fetching profile information: " + request.error);
        }
    }

    //Put Request
    public static IEnumerator PutDetails(string url, string jwtToken, string jsonData)
    {
        // Convert JSON data to byte array
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Create UnityWebRequest for PUT request
        UnityWebRequest request = UnityWebRequest.Put(url, myData);
        request.method = UnityWebRequest.kHttpVerbPUT;
        //www.method = UnityWebRequest.kHttpVerbPUT;
        request.SetRequestHeader("accept", "*/*");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                yield return true;
            }
        }
        else
        {
            Debug.LogError("Error while updating player details: " + request.error);
            yield return false;
        }
    }

    //Link to questionnaire
    public static IEnumerator GetQuestionnaireStatus(string apiUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                string responseBody = request.downloadHandler.text;
                yield return responseBody;
            }
        }
        else
        {
            Debug.LogError("Error fetching profile information: " + request.error);
        }
    }

    private static string responseBody;

    /*public string GetDisplayUnits(string apiUrl, string jwtToken, string frequence, Action<string> callback = null, int year = 2024, string month = null)
    {
        StartCoroutine(GetUnits(apiUrl, jwtToken, frequence, callback, year, month));
        return responseBody;
    }*/

    public static IEnumerator GetUnits(string apiUrl, string jwtToken, string frequence, int year = 2024, string month = null)
    {
        string url = apiUrl;
        switch (frequence)
        {
            case "Yearly":
                url = $"{apiUrl}?year={year}";
                break;
            case "SpecificMonth":
                url = $"{apiUrl}?year={year}&month={month}";
                break;
            case "CurrentState":
                url = $"{apiUrl}";
                break;
        }
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                responseBody = request.downloadHandler.text;
                yield return responseBody;
                //callback?.Invoke(responseBody);
            }
        }
        else
        {
            Debug.LogError("Error fetching profile information: " + request.error);
        }
    }



}
