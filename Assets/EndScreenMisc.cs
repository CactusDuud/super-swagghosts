using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenMisc : MonoBehaviour
{
    public GameObject txt;

    void Start() 
    {
        txt.GetComponent<UnityEngine.UI.Text>().text = TempGameLoop.winner;
    }

    public void toMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
