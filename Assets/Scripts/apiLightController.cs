using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // Import the namespace for UnityWebRequest

public class apiLightController : MonoBehaviour
{
    // Serialized field variable for overall illuminance
    private float overallIlluminance = 1.0f; // Initialize with a default value
    private float updatedScore;

    // Serialized field for an array of parent GameObjects containing lights
    [SerializeField] private GameObject[] parentGameObjects;

    // List to hold references to all the point lights in the specified parent GameObjects and their children
    private List<Light> pointLights = new List<Light>();

    void Start()
    {
        // Find and store references to all the point lights in the specified parent GameObjects and their children
        foreach (GameObject parent in parentGameObjects)
        {
            if (parent != null)
            {
                Light[] lightsInParent = parent.GetComponentsInChildren<Light>();
                pointLights.AddRange(lightsInParent);
            }
            else
            {
                Debug.LogError("One of the parent GameObjects is not assigned.");
            }
        }

        // Start coroutine to fetch overall illuminance from endpoint
        StartCoroutine(GetOverallIlluminance());
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
                // Parse the response and set the overall illuminance
                overallIlluminance = response.score * 1.2f;

                if (PlayerPrefs.GetString("InitialScore") == "False")
                {
                    PlayerPrefs.SetFloat("TotalScore", response.score * 1000);
                    PlayerPrefs.SetString("InitialScore", "True");
                }
                }
            else
            {
                Debug.LogError("Failed to fetch overall illuminance: " + webRequest.error);
            }
        }
    }

    void Update()
    {   
        updatedScore = PlayerPrefs.GetFloat("TotalScore") / 1000;

        if (updatedScore > 0 && updatedScore <= 0.3f)
        {
            overallIlluminance = 0.3f;
            Debug.Log("Score between 0 and 30, Overall Illuminance: " + overallIlluminance);
        }
        else if (updatedScore > 0.3 && updatedScore <= 0.6)
        {
            overallIlluminance = 0.7f;
            Debug.Log("Score between 30 and 60, Overall Illuminance: " + overallIlluminance);
        }
        else
        {
            overallIlluminance = 1.0f;
            Debug.Log("Score greater than 60, Overall Illuminance: " + overallIlluminance);
        }

        // Loop through each point light and adjust its intensity based on the overall illuminance
        foreach (Light light in pointLights)
        {
            // Adjust the intensity of the light based on the overall illuminance
            light.intensity = overallIlluminance;
        }
    }
}

[System.Serializable]
public class ApiResponse
{
    public float score;
    public bool isFinished;
}
