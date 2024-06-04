using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
{
    public GameObject popUpMessage;

    //Show the pop-up message
    public void ClickButton()
    {
        popUpMessage.SetActive(true);
    }
    
    //Close the pop-up message
    public void CloseButton()
    {
        popUpMessage.SetActive(false);
    }
}
