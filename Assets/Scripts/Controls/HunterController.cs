using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HunterController : ParentController
{
    [SerializeField] private GameObject lightAOE;
    private bool power;


    // makes it so function is called every time the special control is activated
    protected override void Awake()
    {
        base.Awake();
        parentControls.Player.Special.performed += _ => PowerLight();
    }

    // turns on a players flashlight if it is off, turns it on if it is on, turns on while holding control
    private void PowerLight()
    {
        if (_view.IsMine)
        {
            if (power)
            {
                lightAOE.SetActive(false);
            }
            else
            {
                lightAOE.SetActive(true);
            }
            power = !power;
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
            if(move != Vector2.zero)
            {
                lightAOE.transform.RotateAround(transform.position, Vector3.forward, Vector3.Angle(lightAOE.transform.up, move));
            }
        }
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(power);
        }
        else
        {
            // Network player, receive data
            power = (bool)stream.ReceiveNext();
        }
    }
}
