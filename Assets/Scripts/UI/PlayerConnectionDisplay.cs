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

    [SerializeField] private TMP_Text playerName;



    private void Awake()
    {
        SetPlayerName("—");
        SetConnectionStatus(false);
    }

    public void SetConnectionStatus(bool isConnected)
    {
        if (isConnected) { _statusLight.color = _connectedColour; }
        else { _statusLight.color = _notConnectedColour; }   
    }

    public void SetPlayerName(string newPlayerName)
    {
        playerName.text = $"{newPlayerName}";

        Debug.Log($"{name}: A player's name was set to \"{newPlayerName}\"");
    }
}
