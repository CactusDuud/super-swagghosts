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
    [SerializeField] private TMP_Text _roomNameDisplay;
    #endregion

    [SerializeField] private byte _maxPlayersPerRoom = 5;
    [SerializeField] private PlayerConnectionDisplay[] _playerDisplays;

    private string _gameVersion = "0.0.1";

    public static NetworkMainManager Instance { get; private set; }


    #region Unity Callbacks
    private void Awake()
    {
        //! Singleton insurance
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        // Ensures that all clients load levels when the parent does.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        // Toggle what UI is available
        _title.SetActive(true);
        _menuPanel.SetActive(true);
        _joinButton.enabled = false;
        _connectionsPanel.SetActive(false);

        // Sets a default name
        _nickname.text = PlayerPrefs.GetString("nickname", "");

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

    public override void OnCreatedRoom()
    {
        Debug.Log($"{name}: Created new room \"{_roomName.text.ToLower()}\"");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{name}: Joined room \"{PhotonNetwork.CurrentRoom.Name}\"");

        JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log($"{name}: Player \"{newPlayer.NickName}\" has entered the room.");

        _playerDisplays[PhotonNetwork.CurrentRoom.PlayerCount].SetPlayerName(newPlayer.NickName);
        _playerDisplays[PhotonNetwork.CurrentRoom.PlayerCount].SetConnectionStatus(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Debug.Log($"{name}: Player \"{otherPlayer.NickName}\" has left the room.");

        // Leave if the host leaves
        if (otherPlayer.IsMasterClient) PhotonNetwork.LeaveRoom();

        ResetPlayerDisplays();
    }
    #endregion


    /// <summary> Joins a room on the Photon Network with the provided name, or creates one if it doesn't exist. </summary>
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected && _roomName.text.ToLower() != "")
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


            Debug.Log($"{name}: Attempting to join room \"{_roomName.text.ToLower()}\"...");

            // Configure settings for the room
            // Ngl this is barely important it just prevents an error
            RoomOptions _roomConfig = new RoomOptions
            {
                MaxPlayers = _maxPlayersPerRoom
            };

            PhotonNetwork.JoinOrCreateRoom(_roomName.text.ToLower(), _roomConfig, null);
        }
    }

    /// <summary> Sets up menu for player selection. </summary>
    private void JoinLobby()
    {
        _title.SetActive(false);
        _menuPanel.SetActive(false);
        _connectionsPanel.SetActive(true);
        _roomNameDisplay.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";

        ResetPlayerDisplays();
    }

    /// <summary> Resets all the player displays in the lobby </summary>
    public void ResetPlayerDisplays()
    {
        int i = 0;
        foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            _playerDisplays[i].Reset();
            _playerDisplays[i].SetPlayerName(p.NickName);
            _playerDisplays[i].SetConnectionStatus(true);
            i++;
        }
    }

    /// <summary> Exits menu for player selection, and disconnects from the room. </summary>
    public void LeaveLobby()
    {
        _connectionsPanel.SetActive(false);
        _title.SetActive(true);
        _menuPanel.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    /// <summary> Loads the level and starts the game. </summary>
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            // Load play area for the master client (automatically synced with all players)
            Debug.Log($"{name}: This is the parent client. Loading level...");
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Map1");
        }
    }

    /// <summary> Close the application. </summary>
    public void QuitGame()
    {
        Debug.Log($"{name}: Closing application...");

        Application.Quit();
    }
}
