using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : ParentHealth
{

    private int iframe_buildup;
    
    // private bool activate_iframe;

    void Awake()
    {
        curr_health = max_health;
        iframe_buildup = 0;
        is_down = false;
        //Debug.Log(curr_health);
    }

    void Update()
    {
        if(curr_health == 0)
        {
            is_down = true;
            GetComponent<GhostController>().enabled = false;
        }
    }

    // resets iframe buildup
    private void ResetIframeBuildUp() {iframe_buildup = 0;}

    //give ghost movement back so that they can get out of the way
    //and they dont take damage as they leave
    // NOTE: i want to increase the speed for a few sec but id have to change the
    // parent controller script and idk if we want to have a public func that can change speed
    private void ActivateInvincibility()
    {
        GetComponent<GhostController>().enabled = true;
        ResetIframeBuildUp();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        // if ghost collides with a flashlight ray, then ghost freezes up, and 
        // takes damage for each second its in the flashlight ray
        // once its taken enough damage it can iframe out of it
        if (collision.tag == "Flashlight")
        {
            GetComponent<GhostController>().enabled = false;


            if(iframe_buildup >= 20){ActivateInvincibility();}
            else{TakeDamage(1);}

            iframe_buildup++;
            Debug.Log(curr_health);
            
        }
    }

    // when the ghost leaves the flashlight ray, the iframe buildup is reset and it can move again
    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetIframeBuildUp();
        GetComponent<GhostController>().enabled = true;
    }
}
