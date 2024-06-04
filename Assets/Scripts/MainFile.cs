using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFile : MonoBehaviour
{
    public Login loginObject;

    //Start the login process
    public void AccessLogin()
    {
        loginObject.GetComponent<Login>().AuthenticatePlayer();
    }
}
