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

    void Start()
    {
        AudioSource heartbeat = GetComponent<AudioSource>();
        heartbeat.Stop();
    }

    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ghost")
        {
            //Debug.DrawLine(transform.position, collision.transform.position, Color.red);
            _view.RPC("ActivateIndicators", RpcTarget.All, true);
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
            _view.RPC("ActivateIndicators", RpcTarget.All, false);
            heartbeat.Stop();
        }
    }


    [PunRPC]
    private void ActivateIndicators(bool on)
    {
        indicator.SetActive(on);
    }
}