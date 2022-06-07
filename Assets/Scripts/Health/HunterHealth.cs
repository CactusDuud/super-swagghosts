using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HunterHealth : ParentHealth
{
    private Animator _anim;

    [SerializeField] private List<GameObject> _hearts; ///

    private int _currHeartNum;

    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();

        _hearts.Add(GameObject.Find("Full Heart1")); ///
        _hearts.Add(GameObject.Find("Full Heart2")); ///
        _hearts.Add(GameObject.Find("Full Heart3")); ///
        _currHeartNum = 3;  ///
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // When a hunter collides with a ghost, hunter's health falls to 0 and is downed
            if(curr_health != 0)
            {
                Debug.Log("collided");
                // TakeDamage(100);
                TakeDamage(34);
               
                _hearts[_currHeartNum-1].SetActive(false);    ///
                _currHeartNum--;    ///
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
            _anim.SetTrigger("isDead");
            Debug.Log("Rigidbody after");
        }
    }
}
