using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LinkToQues : MonoBehaviour
{
    private int NoOfClicks = 0;
    public PopUpMessage popUpMsg;
    public NavigationCommands welcomePage;
    private string currentSceneName;

    public void QuestionnairePage()
    {
        StartCoroutine(LinkToQuestionnaire());
    }

    public IEnumerator LinkToQuestionnaire()
    {
        //Get the questionnaire status 
        CheckQuesCompleted QuesCoroutine = gameObject.AddComponent<CheckQuesCompleted>();
        IEnumerator IsQuesCompleteCoroutine = QuesCoroutine.CheckQuesStatus();
        yield return StartCoroutine(IsQuesCompleteCoroutine);
        bool response = (bool)IsQuesCompleteCoroutine.Current;

        if (response)
        {
            string IsQuestionaireCompleted = PlayerPrefs.GetString("IsQuestionnaireCompleted");

            //If the questionnaire is completed, then go back to the welcome page
            if (IsQuestionaireCompleted == "True")
            {
                currentSceneName = SceneManager.GetActiveScene().name;
                welcomePage.GetComponent<NavigationCommands>().GoToWelcomePage(currentSceneName);
            }
            else
            {
                if (NoOfClicks == 0)
                {
                    NoOfClicks++;
                    LinkToQuestions();      //Link to the Questions page
                }
                else
                {
                    popUpMsg.GetComponent<PopUpMessage>().ClickButton();    //Show the pop-up message if the player clicks the button again without completing the questionnaire
                }

            }
        }
        else
        {
            Debug.Log("Error in authentication ");
        }
    }

    public void LinkToQuestions()
    {
        // Load the Questionnaire web page
        Application.OpenURL("http://localhost:5173/");
    }
}