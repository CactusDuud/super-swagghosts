using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ParentHealth
{

    protected int curr_health;

    [SerializeField] protected int max_health = 100; // can be overrided for variation
    
    void Awake()
    {
        curr_health = max_health;
    }
    
}
