using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMainManager : MonoBehaviourPunCallbacks
{
    #region Menu Elements
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _connectionsPanel;
    [SerializeField] private Button _joinButton;
    [SerializeField] private TMP_InputField _nickname;
    [SerializeField] private TMP_InputField _roomName;
    #endregion

    [SerializeField] private byte _maxPlayersPerRoom = 5;
    [SerializeField] private PlayerConnectionDisplay[] _playerDisplays;

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
        // Toggle what UI is available
        _title.SetActive(true);
        _menuPanel.SetActive(true);
        _joinButton.enabled = false;
        _connectionsPanel.SetActive(false);


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

        
        PlayerSelectScreen();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log($"{name}: Player \"{newPlayer.NickName}\" has entered the room.");

        _playerDisplays[newPlayer.ActorNumber-1].SetPlayerName(newPlayer.NickName);
        _playerDisplays[newPlayer.ActorNumber-1].SetConnectionStatus(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Debug.Log($"{name}: Player \"{otherPlayer.NickName}\" has left the room.");

        _playerDisplays[otherPlayer.ActorNumber-1].SetPlayerName("—");
        _playerDisplays[otherPlayer.ActorNumber-1].SetConnectionStatus(false);
    }
    #endregion


    /// <summary> Joins a room on the Photon Network with the provided name, or creates one if it doesn't exist. </summary>
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected && _roomName.text != "")
        {
            // Determine the player nickname
            if (_nickname.text != "")
            {
                PhotonNetwork.LocalPlayer.NickName = _nickname.text;
                PlayerPrefs.SetString("nickname", _nickname.text);
                Debug.Log($"{name}: Set local nickname to \"{_nickname.text}\"...");
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("nickname", $"Player {PhotonNetwork.LocalPlayer.ActorNumber}");
            }


            Debug.Log($"{name}: Attempting to join room \"{_roomName.text}\"...");

            // Configure settings for the room
            // Ngl this is barely important it just prevents an error
            RoomOptions _roomConfig = new RoomOptions
            {
                MaxPlayers = _maxPlayersPerRoom
            };

            PhotonNetwork.JoinOrCreateRoom(_roomName.text, _roomConfig, null);
        }
    }

    /// <summary> Sets up menu for player selection. </summary>
    private void PlayerSelectScreen()
    {
        _title.SetActive(false);
        _menuPanel.SetActive(false);
        _connectionsPanel.SetActive(true);

        for (int playerNum = 1; playerNum <= PhotonNetwork.CurrentRoom.PlayerCount; playerNum++)
        {
            _playerDisplays[playerNum-1].SetPlayerName(PhotonNetwork.CurrentRoom.Players[playerNum].NickName);
            _playerDisplays[playerNum-1].SetConnectionStatus(true);
        }
    }

    /// <summary> Loads the level and starts the game </summary>
    public void StartGame()
    {
        //TODO: Ensure that everything is ready before allowing this function to be called

        if (PhotonNetwork.IsMasterClient)
        {
            // Load play area for the master client (automatically synced with all players)
            Debug.Log($"{name}: This is the parent client. Loading level...");
            PhotonNetwork.LoadLevel("SceneName");
        }
    }

    /// <summary> Close the application. </summary>
    public void QuitGame()
    {
        Debug.Log($"{name}: Closing application...");

        Application.Quit();
    }
}
