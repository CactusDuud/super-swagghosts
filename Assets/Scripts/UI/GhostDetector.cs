using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDetector : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    AudioSource heartbeat;

    void Start()
    {
        AudioSource heartbeat = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ghost")
        {
            //Debug.DrawLine(transform.position, collision.transform.position, Color.red);
            indicator.SetActive(true);
            heartbeat.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ghost")
        {
            indicator.SetActive(false);
            heartbeat.Stop();
        }
    }
}