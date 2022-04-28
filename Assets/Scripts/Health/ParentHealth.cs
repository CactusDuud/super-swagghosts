using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ParentHealth : MonoBehaviour
{
    protected int curr_health;
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

}