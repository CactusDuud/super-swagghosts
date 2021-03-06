using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;   
public class GhostHealth : ParentHealth
{
    private float iframe_buildup;
    private Rigidbody2D rb;

    // opacity is made so it is opaque at 1 and transparent at 0, anything above 1 will cause it to take longer to become transparent
    [Range(0f,1f)] [ReadOnly] private float _opacity = 1f; 
    [Range(0f,1f)] [SerializeField] private float _fadeSpeed = 0.2f;
    private GhostController _controller;
    private SpriteRenderer _sprite;

    [SerializeField] private GameObject _healthTextObj;

    [PunRPC]
    protected override void RPC_SetHealth(int health)
    {
        base.RPC_SetHealth(health);
    }

    protected override void Awake()
    {
        base.Awake();

        _controller = GetComponent<GhostController>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        iframe_buildup = 0f;

        if(PhotonNetwork.IsMasterClient)
        {
            _healthTextObj = GameObject.Find("Ghost Health Num");
            _healthTextObj.GetComponent<Text>().text = curr_health.ToString();
        }
    }

    private void Update()
    {
        if (Pause.Instance.IsPaused()) return;
        if (curr_health == 0)
        {
            is_down = true;
            _controller.DisableSpookBox();
            _controller.enabled = false;
        }

        this.photonView.RPC("DecreaseOpacity", RpcTarget.All);
        if(PhotonNetwork.IsMasterClient)
        {
            _healthTextObj.GetComponent<Text>().text = curr_health.ToString();
        }

    }

    /// <summary>
    /// Give ghost movement back so that they can get out of the way
    /// and they dont take damage as they leave.
    /// NOTE: i want to increase the speed for a few sec but id have to change the
    /// parent controller script and idk if we want to have a public func that can change speed
    /// </summary>
    private void ActivateInvincibility()
    {
        _controller.enabled = true;
        _controller.EnableSpookBox();   
        iframe_buildup = 0f;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        this.photonView.RPC("SetOpacity", RpcTarget.All, 1f);
    }

    [PunRPC]
    public void SetOpacity(float opacity)
    {
        _opacity = opacity;
    }


    [PunRPC]
    private void DecreaseOpacity()
    {
        // Reduce opacity per second
        // for some reason when time.deltatime is included the ghost doesnt disappear gradually but takes a bit then disappears all at once
        if (_opacity > 0f && !PhotonNetwork.IsMasterClient) _opacity -= _fadeSpeed * Time.deltaTime;
        
        // Set the actual opacity
        UpdateOpacity();
    }

    private void UpdateOpacity()
    {
        _sprite.color = new Color(
            _sprite.color.r, 
            _sprite.color.g, 
            _sprite.color.b, 
            _opacity
            );
    }

    public void TouchLight()
    {
        // If the ghost collides with a light ray, then it freezes up and 
        // takes damage for each second it is in the flashlight ray
        // Once it has taken enough damage, it gets temporary invincibility
        // to escape
        if (iframe_buildup < 1f)
        {
            
            _controller.enabled = false;
            _controller.DisableSpookBox();
            rb.velocity = new Vector3(0, 0, 0);
            TakeDamage(1);
            iframe_buildup = iframe_buildup + (Time.deltaTime / 2f);
        }
        else if (iframe_buildup < 2f)
        {
            
            ActivateInvincibility();
            iframe_buildup = iframe_buildup + (Time.deltaTime/2f);
        }
        else
        {
            _controller.enabled = false;
            _controller.DisableSpookBox();
            rb.velocity = new Vector3(0, 0, 0);
            iframe_buildup = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Trigger invincibility immediately if the ghosts leaves the light ray
        if (collision.gameObject.CompareTag("Flashlight")) ActivateInvincibility();
    }
}
