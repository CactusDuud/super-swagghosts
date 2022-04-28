using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawn;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        //! Singleton insurance
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }
    }

    void Start()
    {
        // Works like instantiate locally, but tells other clients to spawn a player in their view.
        // Basically, call once for yourself and everyone else will also see you.
        PhotonNetwork.Instantiate(_playerPrefab.name, _playerSpawn.position, _playerSpawn.rotation);
    }

    void Update()
    {
        
    }
}
