using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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

    [SerializeField] private Light2D _mainLight;
    [SerializeField] private float _flashMinTime;
    [SerializeField] private float _flashTimeVariance;
    [SerializeField] private int _flashIntensityMulti;
    [ReadOnly] private float _flashCurrentTime;

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _ghostUI;   ///
    [SerializeField] private GameObject _hunterUI;   ///
    private PhotonView _view;

    private void Awake()
    {
        //! Singleton insurance
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        _view = GetComponent<PhotonView>();

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
            GameObject playerToSpawn = _playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            _spawned = PhotonNetwork.Instantiate(playerToSpawn.name, _playerSpawn.position, _playerSpawn.rotation);
            Debug.Log($"{name}: instantiated {_spawned.name}");
        }
        
        _camera.Follow = _spawned.transform;
        _camera.LookAt = _spawned.transform;
        _batteryCurrentTime = _batteryMaxTime;

        _ghostUI.SetActive(false);  ///
        _hunterUI.SetActive(false); ///
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnBattery();

            _view.RPC("DoLighting", RpcTarget.All);
            _ghostUI.SetActive(true); ///

            // int downCount = 0;
            // foreach (var p in GameObject.FindGameObjectsWithTag("Player"))
            // {
            //     if (p.GetComponent<HunterHealth>().is_down)
            //     {
            //         downCount++;
            //     }
            // }

            // if (downCount >= PhotonNetwork.CurrentRoom.PlayerCount - 1)
            // {
            //     Debug.Log("End Game");

            //     Application.Quit();
            // }
        }
        else {_hunterUI.SetActive(true);}   ///
    }

    private void SpawnBattery()
    {
        if (_battery != null) _batteryCurrentTime = _batteryMaxTime;
        else if (_batteryCurrentTime <= 0)
        {
            int spawnIndex = Random.Range(0, 6);
            _battery = PhotonNetwork.Instantiate(_batteryPrefab.name, _batterySpawn[spawnIndex].position, _batterySpawn[spawnIndex].rotation);
            _batteryCurrentTime = _batteryMaxTime;
        }
        else _batteryCurrentTime -= 1f * Time.deltaTime;
    }


    [PunRPC]
    private void DoLightning()
    {
        if (_flashCurrentTime <= 0)
        {
            Debug.Log("lightning execute");
            StartCoroutine(LightningFlashes());
            _flashCurrentTime = _flashMinTime + (_flashTimeVariance * Random.Range(0f, 1f));
        }
        else _flashCurrentTime -= 1f * Time.deltaTime;
    }

    IEnumerator LightningFlashes()
    {
        int _flashCount = 3;

        for (int i = 0; i < _flashCount; i++)
        {
            // High intensity for a few frames
            _mainLight.intensity *= _flashIntensityMulti;
            for (int j = 0; j < Random.Range(2, 7); j++) yield return null;

            // Off for a few frames
            _mainLight.intensity /= _flashIntensityMulti;
            for (int j = 0; j < Random.Range(1, 3); j++) yield return null;
        }

        yield return null;
    }
}
