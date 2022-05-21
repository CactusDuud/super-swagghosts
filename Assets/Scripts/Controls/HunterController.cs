using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class HunterController : ParentController
{
    [SerializeField] private GameObject _lights;
    private Light2D _focusedLight;
    private Light2D _ambientLight;
    private PolygonCollider2D _focusedLightCollider;
    private float _focusedLightDefaultIntensity;
    private float _ambientLightDefaultIntensity;
    
    [SerializeField] private float _lightFuelMax;
    [ReadOnly] [SerializeField] private float _lightFuel;
    [SerializeField] private float _minimumLight;
    private float _focusedLightDefaultDistance;



    protected override void Awake()
    {
        base.Awake();

        _lightFuel = _lightFuelMax;

        _focusedLight = _lights.transform.GetChild(0).GetComponent<Light2D>();
        _focusedLightDefaultIntensity = _focusedLight.intensity;
        _focusedLightDefaultDistance = _focusedLight.pointLightOuterRadius;
        _ambientLight = _lights.transform.GetChild(1).GetComponent<Light2D>();
        _ambientLightDefaultIntensity = _ambientLight.intensity;
        _focusedLightCollider = _lights.transform.GetChild(0).GetComponent<PolygonCollider2D>();
        _focusedLightCollider.enabled = false;

        // Subscribes _isLightOn to the special button being used
        parentControls.Player.Special.performed += _ => PowerLight(true);
        parentControls.Player.Special.canceled += _ => PowerLight(false);

        // Turn off light just in case
        PowerLight(false);
    }

    protected override void MoveEntity()
    {
        base.MoveEntity();

        if (rb.velocity.magnitude >= speed * _minMoveThreshhold)
        {
            _lights.transform.RotateAround(
                transform.position,
                Vector3.forward,
                Vector3.Angle(_lights.transform.up, rb.velocity)
                );
        }
    }

    [PunRPC]
    public void DebugStuff()
    {
        Debug.Log($"Debug Stuff id {GetComponent<PhotonView>().GetInstanceID()}");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        
    }

    private void PowerLight(bool isLightOn)
    {        
        // Handle light logic
        if (isLightOn)
        {
            // Toggle light
            _focusedLight.intensity = _focusedLightDefaultIntensity;
            _focusedLightCollider.enabled = true;
            _ambientLight.intensity = 0;

            // Reduce fuel amount
            if (_lightFuel > 0)
            {
                _lightFuel -= Time.deltaTime;
                if (_lightFuel < 0) { _lightFuel = 0; }
            }

            // Set size of beam 
            // Assumes default scale for hitbox is 1
            float _lightScale = 1 - ((1 - _minimumLight) * (1 - (_lightFuel / _lightFuelMax)));
            _focusedLightCollider.transform.localScale = new Vector3(_lightScale, _lightScale);
            _focusedLight.pointLightOuterRadius = _focusedLightDefaultDistance * _lightScale;
        }
        else
        {
            // Toggle light
            _focusedLight.intensity = 0;
            _focusedLightCollider.enabled = false;
            _ambientLight.intensity = _ambientLightDefaultIntensity;
        }

        // Update over network
        Hashtable hash = new Hashtable();
        hash.Add("isLightOn", isLightOn);
        hash.Add("lightFuel", _lightFuel);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void Refuel(float fuelAmount)
    {
        SetFuel(_lightFuel + fuelAmount);
    }

    public void SetFuel(float fuelAmount)
    {
        _lightFuel = fuelAmount;
        if (_lightFuel < 0) { _lightFuel = 0; }
        if (_lightFuel > _lightFuelMax) { _lightFuel = _lightFuelMax; }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!_view.IsMine && targetPlayer == _view.Owner)
        {
            PowerLight((bool)changedProps["isLightOn"]);
            SetFuel((float)changedProps["lightFuel"]);
        }
    }
}
