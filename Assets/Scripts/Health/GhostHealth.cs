using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GhostHealth : ParentHealth
{
    private int iframe_buildup;

    // opacity is made so it is opaque at 1 and transparent at 0, anything above 1 will cause it to take longer to become transparent
    [Range(0f,1f)][ReadOnly] private float _opacity = 1f; 
    private GhostController _controller;
    private SpriteRenderer _sprite;

    [PunRPC]
    protected override void RPC_SetHealth(int health)
    {
        base.RPC_SetHealth(health);
    }

    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<GhostController>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        //Debug.Log(curr_health);
        if (curr_health == 0)
        {
            is_down = true;
            _controller.enabled = false;
        }

        this.photonView.RPC("DecreaseOpacity", RpcTarget.All);
    }

    /// <summary>
    /// Give ghost movement back so that they can get out of the way
    /// and they dont take damage as they leave.
    /// NOTE: i want to increase the speed for a few sec but id have to change the
    /// parent controller script and idk if we want to have a public func that can change speed
    /// </summary>
    private void ActivateInvincibility()
    {
        _controller.enabled = true;
        _controller.EnableSpookBox();
        _controller.Flee();
        iframe_buildup = 0;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        _opacity = 1f;
    }

    [PunRPC]
    private void DecreaseOpacity()
    {
        //Debug.Log("Hi");
        // Don't change opacity if this is my view
        //if (_view.IsMine) return;

        // Reduce opacity per second
        // for some reason when time.deltatime is included the ghost doesnt disappear gradually but takes a bit then disappears all at once
        if (_opacity > 0f && !PhotonNetwork.IsMasterClient) _opacity -= 0.1f * Time.deltaTime;
        //Debug.Log(_opacity);
        // Set the actual opacity
        UpdateOpacity();
    }

    private void UpdateOpacity()
    {
        //Debug.Log("Hi 2");
        _sprite.color = new Color(
            _sprite.color.r, 
            _sprite.color.g, 
            _sprite.color.b, 
            _opacity
            );
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        // If the ghost collides with a light ray, then it freezes up and 
        // takes damage for each second it is in the flashlight ray
        // Once it has taken enough damage, it gets temporary invincibility
        // to escape
        //Debug.Log("trigger activated");
        if (collision.tag == "Flashlight")
        {
            //Debug.Log("flashlight happening");
            _controller.enabled = false;
            _controller.DisableSpookBox();

            if (iframe_buildup >= 20) ActivateInvincibility();
            else TakeDamage(1);

            iframe_buildup++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Trigger invincibility immediately if the ghosts leaves the light ray
        ActivateInvincibility();
    }
}
