//Functions which are used to navigate to different pages in the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationCommands : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public CheckQuesCompleted linkToQues;
    public PopUpMessage popUpMsg;

    private string currentSceneName;
    private string IsQuestionnaireCompleted;

    //When the player wants to go back to the welcome page
    public void GoToWelcomePage(string currentScene)
    {
        if (currentScene == "Login" || currentScene == "DisplayPDetails")
        {
            StartCoroutine(LoadWelcomePage());
        }
        else
        {
            sceneLoader.GetComponent<SceneLoader>().LoadWelcomeWindow();
        }
    }

    private IEnumerator LoadWelcomePage()
    {
        //Check if the player details are complete
        PDetailesComplete pdetailesComplete = gameObject.AddComponent<PDetailesComplete>();
        IEnumerator playerDetailsCoroutine = pdetailesComplete.AuthenticateAndGetProfile();
        yield return StartCoroutine(playerDetailsCoroutine);
        Debug.Log("playerDetailsCoroutine: " + playerDetailsCoroutine);
        bool IsPlayerDetailsComplete = (bool)playerDetailsCoroutine.Current;

        Debug.Log("IsPlayerDetailsComplete: " + IsPlayerDetailsComplete);

        if (IsPlayerDetailsComplete)
        {
            sceneLoader.GetComponent<SceneLoader>().LoadWelcomeWindow();    //Load the welcome page
        }
        else
        {
            currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == "DisplayPDetails")
            {
                popUpMsg.GetComponent<PopUpMessage>().ClickButton();   //Show the pop-up message if the player details are not complete
            }
            else
            {
                GoToProfilePage();      //Go to the profile page if the player details are not complete
            }
        }
    }

    //When the player wants to go to the profile page
    public void GoToProfilePage()
    {
        sceneLoader.GetComponent<SceneLoader>().LoadProfilePage();
    }

    //When the player wants to exit the game it directs to the welcome page
    public void Exit()
    {
        //BackToWelcomePage();
        GoToWelcomePage("testScene");
    }

    //When the player wants to continue the game it checks if the questionnaire is complete and then loads the game
    public void Continue()
    {
        StartCoroutine(ContinueGame());
    }

    public void ShowLeaderboard()
    {
        StartCoroutine(CheckQuestionnaireStatusAndLoadLeaderboard());
    }

    private IEnumerator CheckQuestionnaireStatusAndLoadLeaderboard()
    {
        yield return StartCoroutine(GetQuestionnaireStatus());
        if (IsQuestionnaireCompleted == "True")
        {
            SceneManager.LoadScene("Leaderboard");
        }
        else
        {
            popUpMsg.GetComponent<PopUpMessage>().ClickButton();
        }
    }

    private IEnumerator ContinueGame()
    {
        yield return StartCoroutine(GetQuestionnaireStatus());

        if (IsQuestionnaireCompleted == "True")
        {
            sceneLoader.GetComponent<SceneLoader>().LoadGame(); //If the questionnaire is complete, load the game
        }
        else
        {
            sceneLoader.GetComponent<SceneLoader>().LoadQuestionnairePage();    //If the questionnaire is not complete, load the questionnaire page
        }
    }

    private IEnumerator GetQuestionnaireStatus()
    {
        //Check if the questionnaire is complete
        CheckQuesCompleted quesDetails = gameObject.AddComponent<CheckQuesCompleted>();
        IEnumerator QuestionnaireCoroutine = quesDetails.CheckQuesStatus();
        yield return StartCoroutine(QuestionnaireCoroutine);

        //Set the questionnaire status in the player prefs
        IsQuestionnaireCompleted = PlayerPrefs.GetString("IsQuestionnaireCompleted");
    }
}
