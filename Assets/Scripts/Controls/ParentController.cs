using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class ParentController : MonoBehaviourPunCallbacks
{
    // pause stuff
    [SerializeField] GameObject pauseMenu;
    [ReadOnly] private bool pause;
    [ReadOnly] private bool playerWhoPaused;

    // Input Actions for controls
    protected ParentControls parentControls;
    protected PhotonView _view;
    protected Rigidbody2D rb;

    // Speed-related variables
    [SerializeField] protected float speed;
    [SerializeField] protected float _minMoveThreshhold = 0.01f;

    protected Vector2 move;


    protected virtual void Awake()
    {
        parentControls = new ParentControls();
        _view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        // Removes rigidbody from network characters (fixes movement jank)
        if (!_view.IsMine) { Destroy(rb); }
    }

    // Enable parentControls
    public override void OnEnable()
    {
        base.OnEnable();

        parentControls.Enable();
    }

    // disable parentControls
    public override void OnDisable()
    {
        base.OnEnable();

        parentControls.Disable();
    }

    /// <summary> Moves the entity based on playerinput. Children also rotate hitboxes to follow their facing direction. </summary>
    protected virtual void MoveEntity()
    {
        move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
        rb.velocity = move;
    }

    protected void PlayerPause()
    {
        if (parentControls.Player.Pause.triggered && Pause.Instance.IsPaused())
        {
            Pause.Instance.UnpauseGame();
        }
        else if (parentControls.Player.Pause.triggered && !Pause.Instance.IsPaused())
        {
            Pause.Instance.PauseGame();
        }
    }

    protected virtual void Update()
    {
        // Originally thought pause wouldnt work with time but i think it is because i forgot to use view.ismine
        // if the way i coded it rn is too difficult to use remind me to try using the time.timescale version but with view.ismine
        if(_view.IsMine)
        {
            PlayerPause();
        }
    }

    // handles movement of the player
    protected virtual void FixedUpdate()
    {
        
        if (!_view.IsMine || Pause.Instance.IsPaused()) return;

        MoveEntity();
    }

}
