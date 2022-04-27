using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMainManager : MonoBehaviourPunCallbacks
{
    /// <summary> TextMeshPro InputField for players to write their desired room name into. </summary>
    [SerializeField] private TMP_InputField _roomName;

    private string _gameVersion = "0.0.1";


    #region Unity Callbacks
    private void Awake()
    {
        // Ensures that all clients load levels when the parent does.
        PhotonNetwork.AutomaticallySyncScene = true;

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
        Debug.Log($"{name}: Connected to Photon servers (region: {PhotonNetwork.CloudRegion}).");

        // TODO: Toggle which buttons are available
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // TODO: Toggle which buttons are available

        Debug.Log($"{name}: Disconnected from room\nCause: {cause}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"{name}: Disconnected from room\nCause: {cause}");
    }
    #endregion

    /// <summary> Creates a room on the Photon Network for </summary>
    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log($"{name}: Joining room \"{_roomName.text}\"...");
            PhotonNetwork.JoinOrCreateRoom(_roomName.text, new RoomOptions(), null);
        }
    }
}
