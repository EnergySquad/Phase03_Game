using UnityEngine;
using System.Collections;
using System;

public class PDetailesComplete : MonoBehaviour
{
    private string apiUrl = "http://20.15.114.131:8080/api/user/profile/view";


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

    // Check if the player's profile details are complete
    bool IsProfileComplete(UserProfile profile)
    {
        return !string.IsNullOrEmpty(profile.firstname)
            && !string.IsNullOrEmpty(profile.lastname)
            && !string.IsNullOrEmpty(profile.username)
            && !string.IsNullOrEmpty(profile.nic)
            && !string.IsNullOrEmpty(profile.phoneNumber)
            && !string.IsNullOrEmpty(profile.email);
    }

    //Check if the player's profile details are complete
    public IEnumerator AuthenticateAndGetProfile()
    {
        string jwtToken = PlayerPrefs.GetString("JWTToken", "");
        Debug.Log("tokenInTheDisplay=" + jwtToken);

        //Call the GetProfile method from AuthenticationManager
        IEnumerator getCoroutine = AuthenticationManager.GetProfile(apiUrl, jwtToken);
        yield return StartCoroutine(getCoroutine);
        string responseBody = getCoroutine.Current as string;

        if (responseBody != null)
        {
            ProfileResponse profileResponse = JsonUtility.FromJson<ProfileResponse>(responseBody);
            bool result = IsProfileComplete(profileResponse.user);
            Debug.Log("result=" + result);
            if (result)
            {
                yield return true;
            }
            else
            {
                yield return false;
            }
        }
        else
        {
            Debug.LogError("Error fetching profile information ");
            yield return false;
        }
    }


}