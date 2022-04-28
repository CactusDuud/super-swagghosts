using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ParentHealth
{

    // [SerializeField] protected int max_health = 100; // can be overrided for variation


    // sets max health to current health
    void Awake()
    {
        curr_health = max_health;
        Debug.Log(curr_health);
    }

    // subtract damage from current health
    // protected override void TakeDamage(int damage)
    // {
    //     curr_health -= damage;
    // }

    //when collides with a ghost, player's health falls to 0 and player can't move
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            if(curr_health != 0)
            {
                TakeDamage(100);
                GetComponent<HunterController>().enabled = false;
                Debug.Log(curr_health);
            }
        }
    }
    
}
