using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : ParentController
{
    [SerializeField] private GameObject lightAOE;
    private bool power;

    protected override void Awake()
    {
        base.Awake();
        parentControls.Player.Special.performed += _ => PowerLight();
    }

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

    protected override void MoveEntity()
    {
        if (_view.IsMine)
        {
            Vector2 move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
            rb.velocity = move;
            if(move != Vector2.zero  )
            {
                lightAOE.transform.RotateAround(transform.position, Vector3.forward, Vector3.Angle(lightAOE.transform.up, move));
            }
        }
    }
}
