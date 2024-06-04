using System.Runtime.CompilerServices;
using UnityEngine;

public class EnergyShieldColor : MonoBehaviour
{
    public Light targetLight;
    public float colorScore;

    private float descent;
    public ScoreCalculator ScoreCalculator;

    void Start(){
        
    }

    void Update()
    {
        descent = ScoreCalculator.GetComponent<ScoreCalculator>().GetDecreasingConst();
        // colorScore = PlayerPrefs.GetFloat("TotalScore");
        // Check if the target light is assigned
        if (targetLight != null)
        { if (descent > 10 && descent < 17) {
            Debug.Log(descent);
            // Change the color of the light
            targetLight.color = Color.magenta;
            } else if (descent > 17) {
            Debug.Log(descent);
                targetLight.color = Color.red;
            }
        }
        else
        {
            Debug.LogWarning("No light assigned to targetLight.");
        }
    }
}
