using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterHealth : ParentHealth
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // When a hunter collides with a ghost, hunter's health falls to 0 and is downed
            if(curr_health != 0)
            {
                TakeDamage(100);
                GetComponent<HunterController>().enabled = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                is_down = true;
            }
        }
    }
}
