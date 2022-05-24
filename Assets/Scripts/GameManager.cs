using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private Transform _ghostSpawn;
    [SerializeField] private GameObject _batteryPrefab;
    [SerializeField] private Transform[] _batterySpawn;
    [SerializeField] private float _batteryMaxTime;
    [ReadOnly] private float _batteryCurrentTime;
    private GameObject _battery;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {


        //! Singleton insurance
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        // Works like instantiate locally, but tells other clients to spawn a player in their view.
        // Basically, call once for yourself and everyone else will also see you.
        // Instantiate a ghost only for the host
        GameObject _spawned;
        if (PhotonNetwork.IsMasterClient)
        {
            _spawned = PhotonNetwork.Instantiate(_ghostPrefab.name, _ghostSpawn.position, _ghostSpawn.rotation);
            Debug.Log($"{name}: instantiated {_spawned.name}");
        }
        else
        {
            //_spawned = PhotonNetwork.Instantiate(_playerPrefab.name, _playerSpawn.position, _playerSpawn.rotation);
            GameObject playerToSpawn = _playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            _spawned = PhotonNetwork.Instantiate(playerToSpawn.name, _playerSpawn.position, _playerSpawn.rotation);
            Debug.Log($"{name}: instantiated {_spawned.name}");
        }
        
        _camera.Follow = _spawned.transform;
        _camera.LookAt = _spawned.transform;
        _batteryCurrentTime = _batteryMaxTime;
    }

    private void SpawnBattery()
    {
        if(_battery != null)
        {
            _batteryCurrentTime = _batteryMaxTime;
        }
        else if(_batteryCurrentTime <= 0)
        {
            int spawnIndex = Random.Range(0, 6);
            _battery = PhotonNetwork.Instantiate(_batteryPrefab.name, _batterySpawn[spawnIndex].position, _batterySpawn[spawnIndex].rotation);
            _batteryCurrentTime = _batteryMaxTime;
        }
        else
        {
            _batteryCurrentTime -= 1f * Time.deltaTime;
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnBattery();

            int downCount = 0;
            foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.GetComponent<HunterHealth>().is_down)
                {
                    downCount++;
                }
            }

            if (downCount >= PhotonNetwork.CurrentRoom.PlayerCount - 1)
            {
                Debug.Log("End Game");

                Application.Quit();
            }
        }
    }
}
