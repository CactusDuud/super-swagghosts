using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;

public class HunterController : ParentController
{
    [SerializeField] private GameObject _lights;
    private Light2D _focusedLight;
    private Light2D _ambientLight;
    private PolygonCollider2D _focusedLightCollider;
    private float _focusedLightDefaultIntensity;
    private float _ambientLightDefaultIntensity;

    private bool _isLightOn;
    [SerializeField] private float _lightFuelMax;
    [ReadOnly] [SerializeField] private float _lightFuel;
    [SerializeField] private float _minimumLight;
    private float _focusedLightDefaultDistance;

    public GameObject optionsMenu;
    public GameObject dimImage;



    public void Refuel(float fuelAmount)
    {
        _lightFuel += fuelAmount;
        if (_lightFuel > _lightFuelMax) { _lightFuel = _lightFuelMax; }
    }

    // turns on a players flashlight if it is off, turns it on if it is on, turns on while holding control
    private void PowerLight()
    {
        if (_view.IsMine)
        {
            if (_isLightOn)
            {
                // Toggle light
                _focusedLight.intensity = _focusedLightDefaultIntensity;
                _focusedLightCollider.enabled = true;
                _ambientLight.intensity = 0;

                // Reduce fuel amount
                if (_lightFuel > 0)
                {
                    _lightFuel -= Time.deltaTime;
                    if (_lightFuel < 0) { _lightFuel = 0; }
                }

                // Set size of beam 
                // Assumes default scale for hitbox is 1
                float _lightScale = 1 - ((1 - _minimumLight) * (1 - (_lightFuel / _lightFuelMax)));
                _focusedLightCollider.transform.localScale = new Vector3(_lightScale, _lightScale);
                _focusedLight.pointLightOuterRadius = _focusedLightDefaultDistance * _lightScale;
            }
            else
            {
                // Toggle light
                _focusedLight.intensity = 0;
                _focusedLightCollider.enabled = false;
                _ambientLight.intensity = _ambientLightDefaultIntensity;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _focusedLight = _lights.transform.GetChild(0).GetComponent<Light2D>();
        _focusedLightDefaultIntensity = _focusedLight.intensity;
        _focusedLightDefaultDistance = _focusedLight.pointLightOuterRadius;
        _ambientLight = _lights.transform.GetChild(1).GetComponent<Light2D>();
        _ambientLightDefaultIntensity = _ambientLight.intensity;
        _focusedLightCollider = _lights.transform.GetChild(0).GetComponent<PolygonCollider2D>();
        _focusedLightCollider.enabled = false;

        // Subscribes _isLightOn to the special button being used
        parentControls.Player.Special.performed += _ => _isLightOn = true;
        parentControls.Player.Special.canceled += _ => _isLightOn = false;
        parentControls.Player.Pause.performed += _ => Pause();

        _lightFuel = _lightFuelMax;
    }

    // moves by applying force to a rigidbody and if the character is moving, move the flashlight AOE in
    // the direction the player is moving
    protected override void MoveEntity()
    {
        if (_view.IsMine)
        {
            Vector2 move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
            rb.velocity = move;
            if (move != Vector2.zero)
            {
                _lights.transform.RotateAround(transform.position, Vector3.forward, Vector3.Angle(_lights.transform.up, move));
            }
        }
    }

    private void Pause() 
    {
        if (!optionsMenu.gameObject.activeSelf)
            Time.timeScale = 0f;
        else
            Unfreeze();
        optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        dimImage.gameObject.SetActive(!dimImage.gameObject.activeSelf);
    }

    public void Unfreeze()
    {
        Time.timeScale = 1f;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(_isLightOn);
            stream.SendNext(_lightFuel);
            GetComponent<PhotonView>().RPC("PowerLight", RpcTarget.Others);
        }
        else
        {
            // Network player, receive data
            _isLightOn = (bool)stream.ReceiveNext();
            _lightFuel = (float)stream.ReceiveNext();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PowerLight();
    }
}
