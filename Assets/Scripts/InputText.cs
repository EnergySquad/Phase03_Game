using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputText : MonoBehaviour
{
    public List<string> InputList { get; private set; } = new List<string>();

    public DisplayText displayText; // Reference to the DisplayText script
    public UpdatePlayerDetails updatePlayerDetails; // Reference to the UpdatePlayerDetails script

    public void StoreInputs()
    {
        InputList.Clear();
        InputField[] inputFields = GetComponentsInChildren<InputField>();

        foreach (InputField inputField in inputFields)
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                Debug.Log("Input field(" + inputField.name + ")is empty. Taking value from DisplayText script.");
                // Get the input field name and take the relevant value from DisplayText.cs
                switch (inputField.name)
                {
                    case "FirstNameInputField":
                        InputList.Add(displayText.Name.text);
                        break;
                    case "SecondNameInputField":
                        InputList.Add(displayText.Lastname.text);
                        break;
                    case "NICInputField":
                        InputList.Add(displayText.NIC.text);
                        break;
                    case "PhoneNumberInputField":
                        InputList.Add(displayText.PhoneNumber.text);
                        break;
                    case "EmailInputField":
                        InputList.Add(displayText.Email.text);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                InputList.Add(inputField.text);
            }
        }

        // Call to DisplayInputDetails method
        //DisplayInputDetails();

        // Call the UpdatePlayerDetails script to update the player details
        updatePlayerDetails.GetComponent<UpdatePlayerDetails>().UploadDetails();
    }

    /*public void DisplayInputDetails()
    {
        for (int i = 0; i < InputList.Count; i++)
        {
            Debug.Log("Input " + i + ": " + InputList[i]);
        }
    }*/

    // Clear the input fields 
    public void ClearInputFields()
    {
        InputField[] inputFields = GetComponentsInChildren<InputField>();
        foreach (InputField inputField in inputFields)
        {
            inputField.text = "";
        }
    }
}
