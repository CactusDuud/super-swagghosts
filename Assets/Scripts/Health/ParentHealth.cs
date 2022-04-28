using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentHealth : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // Each enemy can set their max health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    // resets the health to show change
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    virtual public void TakeDamage(int damage)
    {
        // CurrentHealth -= damage;
        // ChangeHealthBar();

        // if (CurrentHealth <= 0)
        // {
        //     Die();
        // }
    }

}
