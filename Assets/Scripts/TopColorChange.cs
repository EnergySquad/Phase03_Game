using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopColorChange : MonoBehaviour
{
    public Renderer[] projRenderers; // Array of renderers for projectors
    public ScoreCalculator scoreCalculator; // Reference to the score calculator
    public GameObject topLights; // Reference to the top lights GameObject
    private double overallIlluminance; // Overall illuminance value

    // Update is called once per frame
    void Update()
    {
        try
        {
            // Get the score from PlayerPrefs
            float score = PlayerPrefs.GetFloat("TotalScore");
            ChangeLightColor(score);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred in Update: {ex.Message}");
        }
    }

    // Change the color of the lights according to the score
    public void ChangeLightColor(float score)
    {
        try
        {
            Light[] lightsInObject = topLights.GetComponentsInChildren<Light>();
            Renderer[] renderersInObject = topLights.GetComponentsInChildren<Renderer>();

            if (score < 100)
            {
                overallIlluminance = (0.01 * score) + 0.5;
                SetLightColor(lightsInObject, renderersInObject, projRenderers, Color.red);
            }
            else if (score >= 100 && score < 500)
            {
                overallIlluminance = (0.0025 * score) + 0.25;
                SetLightColor(lightsInObject, renderersInObject, projRenderers, Color.yellow);
            }
            else if (score >= 500)
            {
                overallIlluminance = (0.002 * score) - 0.5;
                SetLightColor(lightsInObject, renderersInObject, projRenderers, Color.cyan);
            }

            foreach (Light light in lightsInObject)
            {
                // Set light intensity based on overall illuminance
                light.intensity = (float)overallIlluminance;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred in ChangeLightColor: {ex.Message}");
        }
    }

    // Set the color of the lights and renderers
    private void SetLightColor(Light[] lights, Renderer[] renderersInObject, Renderer[] projRenderers, Color color)
    {
        try
        {
            foreach (Light light in lights)
            {
                light.color = color;
            }
            foreach (Renderer renderer in renderersInObject)
            {
                renderer.material.SetColor("_EmissionColor", color);
                renderer.material.color = color;
            }
            foreach (Renderer renderer in projRenderers)
            {
                renderer.material.SetColor("_EmissionColor", color);
                renderer.material.color = color;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred in SetLightColor: {ex.Message}");
        }
    }
}