using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public abstract class ParentHealth : MonoBehaviour, IPunObservable
{
    protected int curr_health;
    public bool is_down;


    [SerializeField] protected int max_health = 100; // can be overrided for variation
    // public Slider slider;
    // public Gradient gradient;
    // public Image fill;

    // // Each enemy can set their max health
    // public void SetMaxHealth(int health)
    // {
    //     slider.maxValue = health;
    //     slider.value = health;

    //     fill.color = gradient.Evaluate(1f);
    // }

    // // resets the health to show change
    // public void SetHealth(int health)
    // {
    //     slider.value = health;
    //     fill.color = gradient.Evaluate(slider.normalizedValue);
    // }

    public void TakeDamage(int damage)
    {
        curr_health -= damage;
    }

    // virtual void TakeDamage(int damage) {}
    // {
    //     CurrentHealth -= damage;
    //     ChangeHealthBar();

    //     if (CurrentHealth <= 0)
    //     {
    //         Die();
    //     }
    // }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(curr_health);
        }
        else
        {
            // Network player, receive data
            curr_health = (int)stream.ReceiveNext();
        }
    }
}
