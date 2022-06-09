using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

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
    private bool _isLightOn;

    private Animator _anim;



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

        _anim = GetComponent<Animator>();

        // Subscribes _isLightOn to the special button being used
        parentControls.Player.Special.performed += _ => SwitchLight(true);
        parentControls.Player.Special.canceled += _ => SwitchLight(false);

        // Turn off light just in case
        SwitchLight(false);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        // Turn off light in case it was on.
        SwitchLight(false);
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

        Vector2 movement = move / speed;

        if (movement != Vector2.zero)
        {
            UpdateAnimation(movement);
        }
        else
        {
            _anim.SetBool("isMoving", false);
        }
    }

    private void UpdateAnimation(Vector2 movement)
    {
        _anim.SetFloat("moveX", movement.x);
        _anim.SetFloat("moveY", movement.y);
        _anim.SetBool("isMoving", true);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ControlLight();
    }

    private void SwitchLight(bool isLightOn)
    {
        _isLightOn = isLightOn;
    }

    private void ControlLight()
    {        
        // Handle light logic
        if (_isLightOn)
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

        if (!_view.IsMine) return;

        // Update over network
        Hashtable hash = new Hashtable();
        hash.Add("isLightOn", _isLightOn);
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
            SwitchLight((bool)changedProps["isLightOn"]);
            SetFuel((float)changedProps["lightFuel"]);
        }
    }
}
