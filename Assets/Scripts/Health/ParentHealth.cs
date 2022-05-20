using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public abstract class ParentHealth : MonoBehaviourPunCallbacks
{
    [ReadOnly] protected int curr_health;
    public bool is_down;
    protected PhotonView _view;


    [SerializeField] protected int max_health = 100; // can be overrided for variation
    // public Slider slider;
    // public Gradient gradient;
    // public Image fill;

    // // Each enemy can set their max health
    // public void SetMaxHealth(int health)
    // {
    //     slider.maxValue = health;
    //     slider.value = health;

    //     fill.color = gradient.Evaluate(1f);
    // }

    // // resets the health to show change
    // public void SetHealth(int health)
    // {
    //     slider.value = health;
    //     fill.color = gradient.Evaluate(slider.normalizedValue);
    // }

    protected virtual void Awake()
    {
        _view = GetComponent<PhotonView>();

        curr_health = max_health;
        is_down = false;
    }

    public void SetHealth(int health)
    {
        curr_health = health;

        if (_view.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("curr_health", curr_health);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void TakeDamage(int damage)
    {
        SetHealth(curr_health - damage);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!_view.IsMine && targetPlayer == _view.Owner)
        {
            SetHealth((int)changedProps["curr_health"]);
        }
    }
}
