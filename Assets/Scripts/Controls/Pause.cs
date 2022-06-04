using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pause : MonoBehaviourPunCallbacks
{
    public GameObject optionsMenu;
    public GameObject backButton;
    public GameObject optionsButton;


    public void PauseGame()
    {
        this.photonView.RPC("PauseGameRPC", RpcTarget.All);
    }

    public void UnpauseGame()
    {
        this.photonView.RPC("UnpauseGameRPC", RpcTarget.All);
    }

    [PunRPC]
    private void PauseGameRPC()
    {
        Time.timeScale = 0f;
        optionsMenu.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(false);
        
    }

    [PunRPC]
    private void UnpauseGameRPC()
    {
        Time.timeScale = 1f;
        optionsMenu.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(true);
    }
}
