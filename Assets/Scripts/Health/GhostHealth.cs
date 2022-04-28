using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : ParentHealth
{

    private int iframe_buildup;
    private bool activate_iframe;

    void Awake()
    {
        curr_health = max_health;
        Debug.Log(curr_health);
    }

    private void ActivateInvincibility()
    {
        //give ghost movement back so that they can get out of the way
        //and they dont take damage as they leave
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Flashlight") //make sure to have ghost freeze for a few secs here too
        {
            TakeDamage(1);
            iframe_buildup++;
            Debug.Log(curr_health);

            if(iframe_buildup == 20) {ActivateInvincibility();}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        iframe_buildup = 0;
    }
}
