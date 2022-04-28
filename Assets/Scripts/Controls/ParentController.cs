using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class ParentController : MonoBehaviour
{
    // Input Actions for controls
    protected ParentControls parentControls;
    protected PhotonView _view;
    protected Rigidbody2D rb;

    // Speed for movement
    [SerializeField] protected float speed;


    // Initiate parentControls
    protected virtual void Awake()
    {
        parentControls = new ParentControls();
        _view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Enable parentControls
    private void OnEnable()
    {
        parentControls.Enable();
    }

    // disable parentControls
    private void OnDisable()
    {
        parentControls.Disable();
    }

    protected virtual void MoveEntity()
    {
        if (_view.IsMine)
        {
            Vector2 move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
            rb.velocity = move;
        }
    }

    // handles movement of the player
    void FixedUpdate()
    {
        MoveEntity();
    }
}
