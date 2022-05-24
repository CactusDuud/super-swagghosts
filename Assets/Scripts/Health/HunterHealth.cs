using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HunterHealth : ParentHealth
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // When a hunter collides with a ghost, hunter's health falls to 0 and is downed
            if(curr_health != 0)
            {
                Debug.Log("collided");
                TakeDamage(100);
               

                
            }
        }
    }

    

    [PunRPC]
    protected override void RPC_SetHealth(int health)
    {
        base.RPC_SetHealth(health);
        if(curr_health == 0)
        {
            is_down = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<HunterController>().enabled = false;
            Debug.Log("Rigidbody after");
        }
    }
}
