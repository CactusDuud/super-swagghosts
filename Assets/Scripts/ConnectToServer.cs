using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte maxPlayersPerRoom = 5;

    private string _gameVersion = "0.0.1";

    private bool _isAttemptingConnection;

    private void Awake()
    {
        // Ensures that all clients load levels when the parent does.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        // Toggle which buttons are available
        Debug.Log("Connecting to PUN servers...");
        PhotonNetwork.ConnectUsingSettings();

        // Check if already connected... 
        if (PhotonNetwork.IsConnected)
        {
            // ... in which case join a random room. Will call OnJoinRandomFailed() if it fails.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // ... otherwise create a connection.
            // Also keep track of the room in so reloading the Launcher doesn't immediately reconnect.
            _isAttemptingConnection = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }
    

}
