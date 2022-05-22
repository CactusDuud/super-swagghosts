using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : ParentHealth
{
    private int iframe_buildup;
    private GhostController _controller;



    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<GhostController>();
    }

    private void Update()
    {
        Debug.Log(curr_health);
        if (curr_health == 0)
        {
            is_down = true;
            _controller.enabled = false;
        }
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
        _controller.BoostSpeed();
        iframe_buildup = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        // If the ghost collides with a light ray, then it freezes up and 
        // takes damage for each second it is in the flashlight ray
        // Once it has taken enough damage, it gets temporary invincibility
        // to escape
        if (collision.tag == "Flashlight")
        {
            _controller.enabled = false;

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
