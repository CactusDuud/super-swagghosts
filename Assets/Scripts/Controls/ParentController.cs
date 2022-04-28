using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class ParentController : MonoBehaviour
{
    // Input Actions for controls
    private ParentControls parentControls;
    private PhotonView _view;

    // Speed for movement
    [SerializeField] private float speed;


    // Initiate parentControls
    private void Awake()
    {
        parentControls = new ParentControls();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // handles movement of the player
    void FixedUpdate()
    {
        if (_view.IsMine)
        {
            Vector3 move = parentControls.Player.Move.ReadValue<Vector2>();
            
            transform.position += move * speed * Time.fixedDeltaTime;
        }

    }
}
