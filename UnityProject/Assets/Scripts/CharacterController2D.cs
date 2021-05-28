using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    protected Rigidbody2D rigid;
    private BoxCollider2D col;

    protected CharacterState state;

    [SerializeField]
    private int groundRays = 6;
    [SerializeField]
    private float bottomRayLength = .01f;
    [SerializeField]
    private float sideRayLength = .02f;

    [SerializeField]
    protected float maxSpeed = 5f;
    [SerializeField]
    protected float jumpForce = 12f;

    [SerializeField]
    private bool debugRays = false;

    protected CharacterCollision collision;

    [SerializeField]
    private float maxFallSpeed = 8f;
    [SerializeField]
    private float airDragMultiplier = 2f;
    [SerializeField]
    private float airControlMultiplier = 10f;

    protected float xVelocity = 0f;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        HandleCollision();
    }

    protected void Move(float currentInput)
    {
        CalculateXVelocity(currentInput);
        rigid.velocity = new Vector2(xVelocity, Mathf.Max(rigid.velocity.y, -maxFallSpeed));
    }

    private void CalculateXVelocity(float currentInput)
    {
        if (!collision.Grounded)
        {
            float currentDrag = maxSpeed * Time.fixedDeltaTime * airDragMultiplier;

            xVelocity = (Mathf.Abs(xVelocity) <= currentDrag) ? 0f
                : xVelocity - (currentDrag * Mathf.Sign(xVelocity));

            xVelocity += Mathf.Clamp(value: currentInput * maxSpeed * Time.fixedDeltaTime * airControlMultiplier,
                min: -maxSpeed - xVelocity,
                max: maxSpeed - xVelocity);
        } else
        {
            xVelocity = currentInput * maxSpeed;
        }
    }

    protected void Jump()
    {
        SoundManager.Main.PlayNewSound(SoundType.playerJump);
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
    }

    protected void ChangeCharacterState(CharacterState newState)
    {
        switch (newState)
        {
            case CharacterState.Grounded:
                break;
            case CharacterState.InAir:
                break;
            case CharacterState.OnWall:
                break;
        }
    }
    private void HandleCollision()
    {
        CheckGrounded();
        CheckWalls();

    }
    private void CheckGrounded()
    {
        Vector2 bottomLeft = col.bounds.center + new Vector3(-col.bounds.extents.x, -col.bounds.extents.y, 0f);
        Vector2 bottomRight = col.bounds.center + new Vector3(col.bounds.extents.x, -col.bounds.extents.y, 0f);

        collision.Grounded = CheckCollisionForSides(bottomLeft, bottomRight, Vector2.down, bottomRayLength);
    }
    private void CheckWalls()
    {
        Vector2 bottomLeft = col.bounds.center + new Vector3(-col.bounds.extents.x, -col.bounds.extents.y, 0f);
        Vector2 topLeft = col.bounds.center + new Vector3(-col.bounds.extents.x, col.bounds.extents.y, 0f);

        Vector2 topRight = col.bounds.center + new Vector3(col.bounds.extents.x, col.bounds.extents.y, 0f);
        Vector2 bottomRight = col.bounds.center + new Vector3(col.bounds.extents.x, -col.bounds.extents.y, 0f);

        collision.rightCollision = CheckCollisionForSides(topRight, bottomRight, Vector2.right, sideRayLength);
        collision.leftCollision = CheckCollisionForSides(bottomLeft, topLeft, Vector2.left, sideRayLength);

    }
    private bool CheckCollisionForSides(Vector2 point1, Vector2 point2, Vector2 direction, float rayLength)
    {

        bool hasHit = false;

        for (int i = 0; i < groundRays; i++)
        {
            if (hasHit) break;

            Vector2 origin = point1 + i * ((point2 - point1) / (groundRays - 1));
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, rayLength);

            if (debugRays)
                Debug.DrawRay(new Vector3(origin.x, origin.y, 0f), direction * rayLength, Color.red, .1f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == col) continue;

                hasHit = true;
            }
        }
        return hasHit;
    }
}

public enum CharacterState
{
    Grounded,
    InAir,
    OnWall
}

public struct CharacterCollision
{
    public delegate void ColEvent();
    public ColEvent OnLandedEvent;
    public ColEvent OnGroundLeftEvent;
    public bool Grounded
    {
        get => grounded; set
        {
            if (grounded != value)
            {
                grounded = value;
                if (grounded)
                {
                    OnLandedEvent?.Invoke();
                }
                else
                {
                    OnGroundLeftEvent?.Invoke();
                }
            }
        }
    }
    private bool grounded;
    public bool rightCollision;
    public bool leftCollision;
    public bool topCollision;
}
