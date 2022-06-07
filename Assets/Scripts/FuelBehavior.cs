using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBehavior : MonoBehaviour
{
    [SerializeField] private float _wiggleDistance = 1;
    [SerializeField] private float _wiggleSpeed = 1;
    private Transform _sprite;

    private void Awake()
    {
        _sprite = transform.GetChild(0);
    }
     
    private void Update()
    {
        float yPosition = Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleDistance;
        _sprite.localPosition = new Vector3(0, yPosition, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
