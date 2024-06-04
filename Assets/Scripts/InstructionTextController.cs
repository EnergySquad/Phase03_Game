using UnityEngine;
using UnityEngine.UI;

public class InstructionTextController : MonoBehaviour
{
    public Text instructionText; // Reference to the Text component
    public string instruction;
    void Start()
    {
        if (instructionText != null)
        {
            instructionText.text = instruction;
        }
        else
        {
            Debug.LogError("InstructionText reference is not set in the inspector.");
        }
    }
}