using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GhostDetector : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    [SerializeField] AudioSource heartbeat;
    [SerializeField] PhotonView _view;
    [SerializeField] HunterController _controller;

    void Start()
    {
        AudioSource heartbeat = GetComponent<AudioSource>();
        heartbeat.Stop();
    }

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ghost" && _view.IsMine)
        {
            //Debug.DrawLine(transform.position, collision.transform.position, Color.red);
            _controller.ActivateIndicators(indicator, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ghost" && _view.IsMine)
        {
            //Debug.DrawLine(transform.position, collision.transform.position, Color.red);
            heartbeat.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ghost" && _view.IsMine)
        {
            _controller.ActivateIndicators(indicator, false);
            heartbeat.Stop();
        }
    }
}