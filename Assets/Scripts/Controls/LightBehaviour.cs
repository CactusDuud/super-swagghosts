using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask _terrainLayer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            // Shoots a ray towards the player. If it collides with a wall,
            //  the rest of this function does nothing.
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                (collision.transform.parent.transform.position - transform.position),
                Vector2.Distance(transform.position, collision.transform.parent.transform.position),
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
