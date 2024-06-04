using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    private string apiUrl = "http://20.15.114.131:8080/api/user/profile/view";
    
    public Text Name;
    public Text Lastname;
    public Text Username;
    public Text NIC;
    public Text PhoneNumber;
    public Text Email;
    public Text MemberStatis;

    [Serializable]
    public class UserProfile
    {
        public string firstname;
        public string lastname;
        public string username;
        public string nic;
        public string phoneNumber;
        public string email;
    }

    [Serializable]
    public class ProfileResponse
    {
        public UserProfile user;
    }

    public void DisplayPlayerDetails()
    {
        StartCoroutine(AuthenticateAndGetProfile());
    }


    private IEnumerator AuthenticateAndGetProfile()
    {
        string jwtToken = PlayerPrefs.GetString("JWTToken", "");

        //Call the GetProfile method from AuthenticationManager
        IEnumerator getCoroutine = AuthenticationManager.GetProfile(apiUrl, jwtToken);
        yield return StartCoroutine(getCoroutine);
        string responseBody = getCoroutine.Current as string;

        if (responseBody != null)
        {
            ProfileResponse profileResponse = JsonUtility.FromJson<ProfileResponse>(responseBody);
            
            // Display the player details
            Name.text = profileResponse.user.firstname;
            Lastname.text = profileResponse.user.lastname;
            Username.text = profileResponse.user.username;
            NIC.text = profileResponse.user.nic;
            PhoneNumber.text = profileResponse.user.phoneNumber;
            Email.text = profileResponse.user.email;
            MemberStatis.text = "False";
        }
        else
        {
            Debug.LogError("Error fetching profile information.");
        }
    }
}