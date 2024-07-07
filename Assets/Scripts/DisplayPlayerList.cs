using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerList : MonoBehaviour
{
    private string apiUrl= "http://20.15.114.131:8080/api/user/profile/list";

    public delegate void PlayerListFetchedHandler(List<HighscoreEntry> highscoreEntries);
    public event PlayerListFetchedHandler OnPlayerListFetched;

    public void DPlayerList()
    {
        StartCoroutine(GetPlayerList());
    }

    private IEnumerator GetPlayerList()
    {
        string jwtToken = PlayerPrefs.GetString("JWTToken", "");

        //Call the GetProfile method from AuthenticationManager
        IEnumerator getCoroutine = AuthenticationManager.GetProfile(apiUrl, jwtToken);
        yield return StartCoroutine(getCoroutine);
        string responseBody = getCoroutine.Current as string;

        if (responseBody != null)
        {
            PlayerProfileList playerProfileList = JsonUtility.FromJson<PlayerProfileList>(responseBody);
            List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();
            
            foreach (var player in playerProfileList.userViews)
            {
                HighscoreEntry entry;
                if (player.username == "oversight_g2")
                {
                    //Change after testing
                    entry = new HighscoreEntry { score = (int)PlayerPrefs.GetFloat("TotalScore",0), name = player.username };
                }
                else
                {
                    switch (PlayerPrefs.GetString("MissionCompleted"))
                    {
                        case "Level0":
                            entry = new HighscoreEntry { score = Random.Range(500, 1000), name = player.username };
                            break;
                        case "Level1":
                            entry = new HighscoreEntry { score = Random.Range(400, 900), name = player.username };
                            break;
                        case "Level2":
                            entry = new HighscoreEntry { score = Random.Range(300, 800), name = player.username };
                            break;
                        case "Level3":
                            entry = new HighscoreEntry { score = Random.Range(200, 700), name = player.username };
                            break;
                        case "Level4":
                            entry = new HighscoreEntry { score = Random.Range(100, 600), name = player.username };
                            break;
                        default:
                            entry = new HighscoreEntry { score = Random.Range(0, 1000), name = player.username };
                            break;
                    }
                }
                highscoreEntries.Add(entry);
            }
            OnPlayerListFetched?.Invoke(highscoreEntries);
        }
        else
        {
            Debug.LogError("Error fetching profile information.");
        }
    }

    [System.Serializable]
    public class PlayerProfile
    {
        public string firstname;
        public string lastname;
        public string username;
        public string nic;
        public string phoneNumber;
        public string email;
    }

    [System.Serializable]
    public class PlayerProfileList
    {
        public List<PlayerProfile>  userViews;
    }
}
