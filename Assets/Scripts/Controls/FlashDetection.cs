using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDetection : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ghost")
        {
            GhostHealth ghost = collision.gameObject.GetComponent<GhostHealth>();
            ghost.TakeDamage(5);
        }
    }
}
