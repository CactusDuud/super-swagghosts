using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreenMisc : MonoBehaviour
{
    public Text txt;

    void Start() 
    {
        txt.text = TempGameLoop.winner;
    }

    public void toMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
