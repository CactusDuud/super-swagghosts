using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class ParentController : MonoBehaviour, IPunObservable
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

        // Removes rigidbody from network characters (fixes movement jank)
        if (!_view.IsMine) { Destroy(rb); }
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

    // function used to move the entity based on wasd input
    // moves through rigidbody by setting velocity
    protected virtual void MoveEntity()
    {
        if (_view.IsMine)
        {
            Vector2 move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
            rb.velocity = move;
        }
    }

    // handles movement of the player
    protected virtual void FixedUpdate()
    {
        MoveEntity();
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}
}
