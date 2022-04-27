using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMainManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _joinButton;
    [SerializeField] private TMP_InputField _nickname;
    [SerializeField] private string _defaultName = "player";
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
        Debug.Log($"{name}: Created new room \"{_roomName.text}\"");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{name}: Joined room \"{_roomName.text}\"");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log($"{name}: Player \"{newPlayer.NickName}\" has entered the room.");

        if (PhotonNetwork.IsMasterClient)
        {
            // Load play area for the master client (automatically synced with all players)
            //Debug.Log($"{name}: This is the parent client. Loading level...");
            //PhotonNetwork.LoadLevel("SceneName");
        }
    }
    #endregion


    /// <summary> Joins a room on the Photon Network with the provided name, or creates one if it doesn't exist. </summary>
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected && _roomName.text != "")
        {
            // Determine the player nickname
            string _playerNickname = "";
            if (_nickname.text != "")
            {
                _playerNickname = _nickname.text;
                PlayerPrefs.SetString("nickname", _nickname.text);
            }
            else
            {
                _playerNickname = PlayerPrefs.GetString("nickname", _defaultName);
            }

            Debug.Log($"{name}: Setting nickname to \"{_playerNickname}\"...");
            PhotonNetwork.LocalPlayer.NickName = _playerNickname;


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
