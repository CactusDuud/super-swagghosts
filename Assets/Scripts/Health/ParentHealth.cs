using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public abstract class ParentHealth : MonoBehaviourPunCallbacks
{
    protected PhotonView _view;

    [SerializeField] protected int max_health = 100; // can be overrided for variation
    [ReadOnly] protected int curr_health;
    public bool is_down;

    // public Slider slider;
    // public Gradient gradient;
    // public Image fill;


    protected virtual void Awake()
    {
        _view = GetComponent<PhotonView>();

        curr_health = max_health;

        //! This stuff might belong in HunterHealth
        //slider.maxValue = max_health;
        //slider.value = curr_health;
        //fill.color = gradient.Evaluate(1f);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("dmg here");
       this.photonView.RPC("RPC_SetHealth", RpcTarget.All, curr_health - damage);
    }

    protected virtual void RPC_SetHealth(int health)
    {
        if (!_view.IsMine) return;

        curr_health = health;
        if (curr_health < 0) curr_health = 0;
        if (curr_health > max_health) curr_health = max_health;

        Debug.Log($"{name}: Health set to {curr_health}");

        //! This stuff might belong in HunterHealth
        //slider.value = curr_health;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
