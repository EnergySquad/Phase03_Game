using System.Collections;
using UnityEngine;
using UnityEngine.Networking; // Import the namespace for UnityWebRequest

public class ParticleSystemController : MonoBehaviour
{
    // Serialized field variable for rate over time
    private float rateOverTime = 0.0f; // Initialize with a default value

    // Reference to the particle system
    private new ParticleSystem particleSystem;

    void Start()
    {
        GameObject particleSystemObject = GameObject.Find("Fog Particle System");

        if (particleSystemObject == null)
        {
            Debug.LogError("Particle system object not found");
            return;
        }

        // Get reference to the particle system component
        particleSystem = GetComponent<ParticleSystem>();

        // Start coroutine to fetch rate over time from endpoint
        StartCoroutine(GetRateOverTime());
    }

    IEnumerator GetRateOverTime()
    {
        // URL of the endpoint to fetch rate over time
        string url = "http://localhost:8080/questions/sendResults";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the GET request
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response using JsonUtility
                string jsonResponse = webRequest.downloadHandler.text;
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(jsonResponse);

                // Extract the "score" value and set it as rate over time
                Debug.Log("Score: " + response.score);
                if (response.score > 0 && response.score <= 0.3)
                {
                    Debug.Log("Score between 0 and 0.3");
                    rateOverTime = 100.0f;
                }
                else if (response.score > 0.3 && response.score <= 0.6)
                {
                    Debug.Log("Score between 0.3 and 0.6");
                    rateOverTime = 50.0f;
                }
                else
                {
                    Debug.Log("Score greater than 0.6");
                    rateOverTime = 10.0f;
                }

                // Update the particle system emission rate
                var emission = particleSystem.emission;
                emission.rateOverTime = rateOverTime;
            }
            else
            {
                Debug.LogError("Failed to fetch rate over time: " + webRequest.error);
            }
        }
    }

    void Update()
    {
        //get the updated total score value form the Scorecalculator.cs file
        float updated_score = PlayerPrefs.GetFloat("TotalScore");
        Debug.Log("Updated Score: " + updated_score);

        //update the rate over time value based on the updated score
        if (updated_score > 0 && updated_score <= 300)
        {
            Debug.Log("Score between 0 and 300");
            rateOverTime = 100.0f;
        }
        else if (updated_score > 300 && updated_score <= 600)
        {
            Debug.Log("Score between 300 and 600");
            rateOverTime = 50.0f;
        }
        else
        {
            Debug.Log("Score greater than 600");
            rateOverTime = 10.0f;
        }

        // Update the particle system emission rate
        var emission = particleSystem.emission;
        emission.rateOverTime = rateOverTime;

    }
}

