using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkRoomManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks
    public override void OnJoinedRoom()
    {
        Debug.Log($"{name}: Joined Museum Manor level.");
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
}
