using System.Collections;
using UnityEngine;

public class GhostController : ParentController
{
    [SerializeField] private Collider2D _spookBox;
    private float baseSpeed;
    [SerializeField] float fleeSpeedBoost = 1.5f;
    [SerializeField] float fleeDuration = 5f;



    protected override void Awake()
    {
        base.Awake();

        baseSpeed = base.speed;
    }

    protected override void MoveEntity()
    {
        if (rb.velocity.magnitude >= speed * _minMoveThreshhold)
        {
            _spookBox.transform.RotateAround(
                transform.position,
                Vector3.forward, 
                Vector3.Angle(_spookBox.transform.up, rb.velocity)
                );
        }
    }

    /// <summary>
    /// Changes the speed of the ghost temporarily
    /// </summary>
    /// <param name="speedMultiplier"> Multiplier on the base speed </param>
    /// <param name="duration"> How long the boost should last </param>
    public void BoostSpeed()
    {
        speed *= fleeSpeedBoost;
        StartCoroutine("WaitALittle", fleeDuration);
        speed = baseSpeed;
    }

    /// <summary>
    /// Waits for a little bit.
    /// </summary>
    /// <param name="duration"> How long to wait, in seconds </param>
    IEnumerable WaitALittle(float duration) { yield return new WaitForSeconds(duration); }
}
