using System.Collections;
using UnityEngine;

public class GhostController : ParentController
{
    [SerializeField] private Collider2D _spookBox;
    private float baseSpeed;
    [SerializeField] float fleeSpeedBoost = 1.5f;
    [SerializeField] float fleeDuration = 5f;
    
    // private Animator _anim; ///


    protected override void Awake()
    {
        base.Awake();

        baseSpeed = base.speed;
        // _anim = GetComponent<Animator>();
    }

    protected override void MoveEntity()
    {
        base.MoveEntity();

        if (rb.velocity.magnitude >= speed * _minMoveThreshhold)
        {
            _spookBox.transform.RotateAround(
                transform.position,
                Vector3.forward, 
                Vector3.Angle(-_spookBox.transform.up, rb.velocity)
                );
            
        }
        if(base.move.y > 1){transform.Rotate (Vector3.back * 90);}
        else if (base.move.y < 1){ transform.Rotate (Vector3.back * -90);}
        if(base.move.x > 1){transform.Rotate (Vector3.forward * 90);}
        else if(base.move.x < 1){transform.Rotate (Vector3.forward * -90);}

    }

    /// <summary>
    /// Changes the speed of the ghost temporarily
    /// </summary>
    /// <param name="speedMultiplier"> Multiplier on the base speed </param>
    /// <param name="duration"> How long the boost should last </param>
    public void Flee()
    {
        speed *= fleeSpeedBoost;
        DisableSpookBox();
        StartCoroutine("WaitALittle", fleeDuration);
        EnableSpookBox();
        speed = baseSpeed;
    }

    public void EnableSpookBox()
    {
        _spookBox.enabled = true;
    }

    public void DisableSpookBox()
    {
        _spookBox.enabled = false;
    }

    /// <summary>
    /// Waits for a little bit.
    /// </summary>
    /// <param name="duration"> How long to wait, in seconds </param>
    IEnumerable WaitALittle(float duration) { yield return new WaitForSeconds(duration); }
}
