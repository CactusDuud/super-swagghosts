using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkRoomManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Photon Callbacks
    public override void OnJoinedRoom()
    {
        Debug.Log($"{name}: Joined room <PLACEHOLDER>.");

        // Room creation logic
        // Character select, map select, and loading the right level
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
