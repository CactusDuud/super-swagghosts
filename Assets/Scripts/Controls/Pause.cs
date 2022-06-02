using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject dimImage;
    public GameObject optionsButton;

    public void PauseGame()
    {
        if (!optionsMenu.gameObject.activeSelf)
        {
            Time.timeScale = 0f;
            optionsMenu.gameObject.SetActive(true);
            dimImage.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            optionsMenu.gameObject.SetActive(false);
            dimImage.gameObject.SetActive(false);
            optionsButton.gameObject.SetActive(false);
        }
    }
}
