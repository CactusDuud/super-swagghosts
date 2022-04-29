using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : ParentController
{
    [SerializeField] private Collider2D _spookBox;

    protected override void MoveEntity()
    {
        if (_view.IsMine)
        {
            Vector2 move = parentControls.Player.Move.ReadValue<Vector2>() * speed;
            rb.velocity = move;
            if (move != Vector2.zero)
            {
                _spookBox.transform.RotateAround(transform.position, Vector3.forward, Vector3.Angle(_spookBox.transform.up, move));
            }
        }
    }


    public void ModifySpeed(float newSpeed)
    {
        speed *= newSpeed;
    }


    
}
