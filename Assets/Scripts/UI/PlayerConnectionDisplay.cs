using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[Serializable]
public class PlayerConnectionDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private Color _notConnectedColour;
    [SerializeField] private Color _connectedColour;
    [SerializeField] private Image _statusLight;
    [SerializeField] private Image _characterImage;

    [SerializeField] private TMP_Text playerName;

    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;

    private Hashtable playerProperties = new Hashtable();
    [SerializeField] private Image playerAvatar;
    [SerializeField] private Sprite[] avatars;

    private Player _player;


    private void Awake()
    {
        Reset();
        playerProperties["playerAvatar"] = 0;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        UpdatePlayerAvatar(player);
    }

    public void SetConnectionStatus(bool isConnected)
    {
        if (isConnected) 
        {
            _statusLight.color = _connectedColour;
            _characterImage.enabled = true;
        }
        else
        {
            _statusLight.color = _notConnectedColour;    
            _characterImage.enabled = false;
        }   
    }

    public void SetPlayerName(string newPlayerName)
    {
        playerName.text = $"{newPlayerName}";
    }

    public void ActivateButtons()
    {
        upButton.SetActive(true);
        downButton.SetActive(true);
    }

    public void Reset()
    {
        SetPlayerName("—");
        SetConnectionStatus(false);
        upButton.SetActive(false);
        downButton.SetActive(false);
    }

    // button functionalities
    public void OnClickUpButton()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickDownButton()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (_player == targetPlayer)
        {
            UpdatePlayerAvatar(targetPlayer);
        }
    }

    private void UpdatePlayerAvatar(Player p)
    {
        if (!p.IsMasterClient)
        {
            if (p.CustomProperties.ContainsKey("playerAvatar"))
            {
                playerAvatar.sprite = avatars[(int)p.CustomProperties["playerAvatar"]];
                playerProperties["playerAvatar"] = (int)p.CustomProperties["playerAvatar"];
            }
            else
            {
                playerProperties["playerAvatar"] = 0;
            }
        }
    }
}
