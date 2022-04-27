using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte maxPlayersPerRoom = 5;
    

    #region Pun Callbacks
    private void OnConnectedToServer()
    {
        Debug.Log($"{name}: Connected to Photon servers ({PhotonNetwork.CloudRegion}).");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"{name}: Joined lobby for Super Swagghosts.");
    }
    #endregion
}
