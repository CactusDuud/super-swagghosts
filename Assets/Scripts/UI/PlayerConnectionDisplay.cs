using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class PlayerConnectionDisplay : MonoBehaviour
{
    [SerializeField] private Color _notConnectedColour;
    [SerializeField] private Color _connectedColour;
    [SerializeField] private Image _statusLight;
    [SerializeField] private Image _characterImage;

    [SerializeField] private TMP_Text playerName;



    private void Awake()
    {
        SetPlayerName("—");
        SetConnectionStatus(false);
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
}
