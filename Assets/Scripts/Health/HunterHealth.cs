using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HunterHealth : ParentHealth
{
    private Animator _anim;
    private Rigidbody2D _rb;
    private HunterController _controller;

    [SerializeField] private List<GameObject> _hearts;

    private int _currHeartNum;

    [SerializeField] private int _reviveThreshold = 100;
    private int _reviveCount = 0;

    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<HunterController>();

        _hearts.Add(GameObject.Find("Full Heart1"));
        _hearts.Add(GameObject.Find("Full Heart2"));
        _hearts.Add(GameObject.Find("Full Heart3"));
        _currHeartNum = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // When a hunter collides with a ghost, hunter's health falls to 0 and is downed
            if (curr_health != 0)
            {
                TakeDamage(100);
                Debug.Log("taken damage");
               
                LoseHeart();
            }
        }

        if (is_down && collision.gameObject.CompareTag("Flashlight"))
        {
            if (_reviveCount < _reviveThreshold) _reviveCount++;
            else
            {
                RPC_SetHealth(max_health);
            }
        }
    }

    private void LoseHeart()
    {
        _hearts[_currHeartNum-1].SetActive(false);
        _currHeartNum--;
    }

    private void GainHeart()
    {
        _hearts[_currHeartNum].SetActive(true);
        _currHeartNum++;
    }

    [PunRPC]
    protected override void RPC_SetHealth(int health)
    {
        base.RPC_SetHealth(health);

        if (curr_health <= 0)
        {
            curr_health = 0;
            is_down = true;
            _reviveCount = 0;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            _controller.enabled = false;
            _anim.SetTrigger("isDead");
        }

        if (curr_health >= max_health)
        {
            curr_health = max_health;
            is_down = false;
            _reviveCount = 0;
            _rb.isKinematic = false;
            _controller.enabled = true;
        }
    }
}
