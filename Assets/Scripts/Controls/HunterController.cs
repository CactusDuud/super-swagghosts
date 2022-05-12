using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;

public class HunterController : ParentController
{
    [SerializeField] private GameObject _lights;
    private GameObject _focusedLight;
    private GameObject _ambientLight;
    private bool _isLightOn;

    public GameObject optionsMenu;
    public GameObject dimImage;


    // Subscribes _isLightOn to the special button being used
    protected override void Awake()
    {
        base.Awake();

        _focusedLight = _lights.transform.GetChild(0).gameObject;
        _ambientLight = _lights.transform.GetChild(1).gameObject;

        parentControls.Player.Special.performed += _ => _isLightOn = true;
        parentControls.Player.Special.canceled += _ => _isLightOn = false;
        parentControls.Player.Pause.performed += _ => Pause();
    }

    // turns on a players flashlight if it is off, turns it on if it is on, turns on while holding control
    private void PowerLight()
    {
        if (_view.IsMine)
        {
            _focusedLight.SetActive(_isLightOn);
            _ambientLight.SetActive(_isLightOn);
        }
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
        }
        else
        {
            // Network player, receive data
            _isLightOn = (bool)stream.ReceiveNext();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PowerLight();
    }
}
