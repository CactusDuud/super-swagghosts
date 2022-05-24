using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class ParentController : MonoBehaviourPunCallbacks
{
    // Input Actions for controls
    protected ParentControls parentControls;
    protected PhotonView _view;
    protected Rigidbody2D rb;

    // Speed-related variables
    [SerializeField] protected float speed;
    [SerializeField] protected float _minMoveThreshhold = 0.01f;

    // Pause menu variables
    public GameObject optionsMenu;
    public GameObject dimImage;



    protected virtual void Awake()
    {
        parentControls = new ParentControls();
        _view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        parentControls.Player.Pause.performed += _ => Pause();

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
        Vector2 move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
        rb.velocity = move;
    }

    // handles movement of the player
    protected virtual void FixedUpdate()
    {
        if (!_view.IsMine) return;

        MoveEntity();
    }

    private void Pause() 
    {
        if (!optionsMenu.gameObject.activeSelf)
        {
            Time.timeScale = 0f;
            optionsMenu.gameObject.SetActive(true);
            dimImage.gameObject.SetActive(true);
        }
        else 
        {
            Time.timeScale = 1f;
            optionsMenu.gameObject.SetActive(false);
            dimImage.gameObject.SetActive(false);
        }
    }
}
