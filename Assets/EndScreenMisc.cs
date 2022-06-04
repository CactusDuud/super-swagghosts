using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenMisc : MonoBehaviour
{
    public TextMeshProUGUI txt;

    void Awake() 
    {
        txt.text = TempGameLoop.winner;
    }

    public void toMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
