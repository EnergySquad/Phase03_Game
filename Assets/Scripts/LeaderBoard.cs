using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryBg;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    public DisplayPlayerList displayPlayerList;

    private void Awake()
    {
        try
        {
            entryBg = transform.Find("playerBoardBg");
            if (entryBg == null)
            {
                Debug.LogError("playerBoardBg not found.");
                return;
            }

            entryContainer = entryBg.Find("highscoreEntryContainer");
            if (entryContainer == null)
            {
                Debug.LogError("highscoreEntryContainer not found.");
                return;
            }

            entryTemplate = entryContainer.Find("highscoreEntryTemplate");
            if (entryTemplate == null)
            {
                Debug.LogError("highscoreEntryTemplate not found.");
                return;
            }

            entryTemplate.gameObject.SetActive(false); // hide the template

            if (displayPlayerList == null)
            {
                Debug.LogError("displayPlayerList is not assigned.");
                return;
            }

            displayPlayerList.OnPlayerListFetched += OnPlayerListFetched;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in Awake: {ex.Message}");
        }
    }

    private void OnEnable()
    {
        try
        {
            // Call DPlayerList to fetch player data every time the leaderboard is enabled
            displayPlayerList.DPlayerList();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in OnEnable: {ex.Message}");
        }
    }

    private void OnPlayerListFetched(List<HighscoreEntry> highscoreEntries)
    {
        try
        {
            if (highscoreEntries == null)
            {
                Debug.LogError("highscoreEntries is null.");
                return;
            }

            highscoreEntryList = highscoreEntries;
            SortHighscores();

            // Clear previous entries
            foreach (Transform child in entryContainer)
            {
                if (child != entryTemplate) // Don't remove the template itself
                {
                    Destroy(child.gameObject);
                }
            }

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in OnPlayerListFetched: {ex.Message}");
        }
    }

    private void SortHighscores()
    {
        try
        {
            highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in SortHighscores: {ex.Message}");
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        try
        {
            if (highscoreEntry == null || container == null)
            {
                Debug.LogError("HighscoreEntry or container is null.");
                return;
            }

            float templateHeight = 70f;
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString = GetRankString(rank, entryTransform);

            entryTransform.Find("posText").GetComponent<Text>().text = rankString;
            entryTransform.Find("scoreText").GetComponent<Text>().text = highscoreEntry.score.ToString();
            entryTransform.Find("nameText").GetComponent<Text>().text = highscoreEntry.name;
            entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

            if (highscoreEntry.name == "oversight_g2")
            {
                HighlightSpecialEntry(entryTransform);
            }

            transformList.Add(entryTransform);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in CreateHighscoreEntryTransform: {ex.Message}");
        }
    }

    private string GetRankString(int rank, Transform entryTransform)
    {
        try
        {
            switch (rank)
            {
                case 1:
                    entryTransform.Find("trophy").GetComponent<Image>().color = Color.yellow;
                    return "1ST";
                case 2:
                    entryTransform.Find("trophy").GetComponent<Image>().color = Color.white;
                    return "2ND";
                case 3:
                    ColorUtility.TryParseHtmlString("#CD8A8A", out Color color3);
                    entryTransform.Find("trophy").GetComponent<Image>().color = color3;
                    return "3RD";
                default:
                    entryTransform.Find("trophy").gameObject.SetActive(false);
                    return rank + "TH";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in GetRankString: {ex.Message}");
            return rank + "TH";
        }
    }

    private void HighlightSpecialEntry(Transform entryTransform)
    {
        try
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in HighlightSpecialEntry: {ex.Message}");
        }
    }
}

[System.Serializable]
public class HighscoreEntry
{
    public int score;
    public string name;
}

