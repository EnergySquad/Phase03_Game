using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpdatePlayerDetails : MonoBehaviour
{
    public string gamingSceneName = "WelcomePage"; // Name of the gaming scene to load
    public InputText inputTextScript; // Reference to your InputText script
    private string url = "http://20.15.114.131:8080/api/user/profile/update";
    public DisplayText displayText;
    public PopUpMessage popUpMsg;

    public void UploadDetails()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        string jwtToken = PlayerPrefs.GetString("JWTToken", "");
        // Get the input details from InputText script
        List<string> inputDetails = inputTextScript.InputList;

        // Construct JSON data using input details
        string jsonData = "{\"firstname\": \"" + inputDetails[0] + "\", " +
                          "\"lastname\": \"" + inputDetails[1] + "\", " +
                          "\"nic\": \"" + inputDetails[2] + "\", " +
                          "\"phoneNumber\": \"" + inputDetails[3] + "\", " +
                          "\"email\": \"" + inputDetails[4] + "\"}";

        // Call the PutDetails method from AuthenticationManager
        IEnumerator putCoroutine = AuthenticationManager.PutDetails(url, jwtToken, jsonData);
        yield return StartCoroutine(putCoroutine);
        bool isUpdated = (bool)putCoroutine.Current;

        // Display the updated player details if the player details are updated successfully
        if (isUpdated)
        {
            // Clear the current input fields
            inputTextScript.ClearInputFields();

            // Display the updated player details
            displayText.GetComponent<DisplayText>().DisplayPlayerDetails();

            Debug.Log("Profile update successful!");
        }
        else
        {
            popUpMsg.GetComponent<PopUpMessage>().ClickButton();
            Debug.LogError("Error while updating player details");
        }
    }
}
