using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Serialized field variable for overall illuminance
    [SerializeField]
    private float overallIlluminance = 1.0f;

    // Array to hold references to all the point lights in the scene
    private Light[] pointLights;

    void Start()
    {
        // Find and store references to all the point lights in the scene
        pointLights = FindObjectsOfType<Light>();
    }

    void Update()
    {
        // Loop through each point light and adjust its intensity based on the overall illuminance
        foreach (Light light in pointLights)
        {
            // Adjust the intensity of the light based on the overall illuminance
            light.intensity = overallIlluminance;
        }
    }
}
