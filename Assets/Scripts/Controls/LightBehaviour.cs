using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _terrainLayer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // Shoots a ray towards the player. If it collides with a wall,
            //  the rest of this function does nothing.
            RaycastHit2D hit = Physics2D.Raycast(
                _player.position,
                (collision.transform.position - _player.position),
                Vector2.Distance(_player.position, collision.transform.position),
                _terrainLayer
            );
            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag("Player")) return;
            }

            collision.GetComponent<GhostHealth>().TouchLight();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<HunterHealth>().Revive();
        }
    }
}
