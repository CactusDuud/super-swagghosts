using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMainManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _joinButton;
    /// <summary> TextMeshPro InputField for players to write their desired room name into. </summary>
    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private byte _maxPlayersPerRoom = 5;

    private string _gameVersion = "0.0.1";


    #region Unity Callbacks
    private void Awake()
    {
        // Ensures that all clients load levels when the parent does.
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log($"{name}: Automatic Scene Synching enabled");
    }

    private void Start()
    {
        // Disable join button until connected
        _joinButton.enabled = false;

        // Immediately try to connect to PUN servers.
        // Also save connection status for 
        Debug.Log($"{name}: Connecting to PUN servers...");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = _gameVersion; 
    }
    #endregion


    #region Pun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log($"{name}: Connected to Photon Cloud (region: {PhotonNetwork.CloudRegion}).");
        
        // Enable the join button to allow players to join a room
        _joinButton.enabled = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"{name}: Disconnected from Photon Cloud\nCause: {cause}");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"{name}: Joined lobby for Super Swagghosts.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"{name}: Created new room \"{_roomName}\"");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{name}: Joined room \"{_roomName}\"");
    }
    #endregion


    /// <summary> Joins a room on the Photon Network with the provided name, or creates one if it doesn't exist. </summary>
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected && _roomName.text != "")
        {
            Debug.Log($"{name}: Joining room \"{_roomName.text}\"...");

            // Configure settings for the room
            // Ngl this is barely important it just prevents an error
            RoomOptions _roomConfig = new RoomOptions
            {
                MaxPlayers = _maxPlayersPerRoom
            };

            PhotonNetwork.JoinOrCreateRoom(_roomName.text, _roomConfig, null);
            //TODO: Load the room menu scene
        }
    }

    /// <summary> Close the application. </summary>
    public void QuitGame()
    {
        Debug.Log($"{name}: Closing application...");

        Application.Quit();
    }
}
