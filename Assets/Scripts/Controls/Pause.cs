using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pause : MonoBehaviourPunCallbacks
{
    public static Pause Instance;

    public GameObject optionsMenu;
    //public GameObject backButton;
    //public GameObject optionsButton;

    [ReadOnly] private bool paused = false;

    private void Awake()
    {
        Instance = this;
    }
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
        //Time.timeScale = 0f;
        paused = true;
        optionsMenu.gameObject.SetActive(true);
        //backButton.gameObject.SetActive(true);
        //optionsButton.gameObject.SetActive(false);
        
    }

    [PunRPC]
    private void UnpauseGameRPC()
    {
        Debug.Log("unpause");
        optionsMenu.gameObject.SetActive(false);
        paused = false;
        //backButton.gameObject.SetActive(false);
        //optionsButton.gameObject.SetActive(true);
        //Time.timeScale = 1f;
    }

    public bool IsPaused()
    {
        return paused;
    }

}
