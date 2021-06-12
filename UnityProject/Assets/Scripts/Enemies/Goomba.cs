using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : CharacterController2D
{
    private bool facingRight = false;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private int damage = 2;

    private void FixedUpdate()
    {
        Move(facingRight ? moveSpeed : -moveSpeed);

        if (collision.leftCollision || collision.rightCollision)
        {
            SetXForce(collision.leftCollision ? moveSpeed : -moveSpeed);
            facingRight = collision.leftCollision;
        }
    }
    public void Turn()
    {
        facingRight = !facingRight;
    }
}
