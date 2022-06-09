using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public abstract class ParentHealth : MonoBehaviourPunCallbacks
{
    protected PhotonView _view;

    [SerializeField] protected int max_health = 100;
    [ReadOnly] protected int curr_health;
    public bool is_down = false;

    protected virtual void Awake()
    {
        _view = GetComponent<PhotonView>();

        curr_health = max_health;
    }

    public virtual void TakeDamage(int damage)
    {
        if(!Pause.Instance.IsPaused())
        {
            this.photonView.RPC("RPC_SetHealth", RpcTarget.All, damage);
        }
        
    }

    protected virtual void RPC_SetHealth(int health)
    {
        curr_health -= health;
        if (curr_health < 0) curr_health = 0;
        if (curr_health > max_health) curr_health = max_health;
    }
}
