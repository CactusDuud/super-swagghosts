using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte maxPlayersPerRoom = 5;
    [SerializeField] private TMP_InputField _roomName;

    private string _gameVersion = "0.0.1";
    private bool _isAttemptingConnection;



    #region Unity Callbacks
    private void Awake()
    {
        // Ensures that all clients load levels when the parent does.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        // TODO: Toggle which buttons are available


        // Create a new room and keep track of the room in so reloading the Launcher doesn't immediately reconnect.
        Debug.Log($"{name}: Connecting to PUN servers...");
        _isAttemptingConnection = PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = _gameVersion;
    }
    #endregion


    public void HostRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.CreateRoom(_roomName.text);
            Debug.Log($"{name}: Created room \"{_roomName.text}\".");

            JoinRoom();
        }
    }

    public void JoinRoom()
    {
        // If the NetworkManager has connected to Photon servers, join a room
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(_roomName.text);
            Debug.Log($"{name}: Joining room \"{_roomName.text}\"...");

            // No longer attempting a connection
            _isAttemptingConnection = false;
        }
    }
    

     #region Pun Callbacks
    public override void OnJoinedLobby()
    {
        Debug.Log($"{name}: Joined lobby for Super Swagghosts.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // TODO: Toggle which buttons are available

        Debug.Log($"{name}: Disconnected from room\nCause: {cause}");

        // Ensure we don't immediately reconnect
        _isAttemptingConnection = false;
    }
    #endregion
}
