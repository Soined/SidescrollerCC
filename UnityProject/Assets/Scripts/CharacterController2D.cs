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
    private float justJumpedTime = .1f;
    private float _justJumpedTime = 0f;
    protected bool justJumped { get => _justJumpedTime >= 0; set => _justJumpedTime = value ? justJumpedTime : 0f; }

    [SerializeField]
    protected float gravityScale = 1f;
    private const float gravity = 9.81f;
    protected Vector2 velocity = new Vector2();

    //Disables all Velocity calculations on X-Axis for this frame
    private bool xForceSetThisFrame = false;
    //Disables all Velocity calculations on Y-Axis for this frame
    private bool yForceSetThisFrame = false;

    [SerializeField]
    private bool debugRays = false;
    [SerializeField]
    protected LayerMask groundLayer;

    protected CharacterCollision collision;

    [SerializeField]
    private float maxFallSpeed = 8f;
    [SerializeField]
    private float airDragMultiplier = 2f;
    [SerializeField]
    private float airControlMultiplier = 10f;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        collision.OnLandedEvent += ResetGravity;
    }

    protected virtual void Update()
    {
        HandleCollision();

        if (justJumped) _justJumpedTime -= Time.deltaTime;
    }

    protected void Move(float currentInput)
    {
        CalculateXVelocity(currentInput);
        CalculateYVelocity();

        xForceSetThisFrame = false;
        yForceSetThisFrame = false;

        rigid.MovePosition(transform.position + (Vector3)velocity * Time.fixedDeltaTime);
    }
    private void CalculateYVelocity()
    {
        if (yForceSetThisFrame) return;
        if (!collision.Grounded)
        {
            velocity.y -= gravity * Time.fixedDeltaTime * gravityScale;
            velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
        }
    }
    private void ApplyInputOnVelocity(float currentInput)
    {
        //TODO
    }

    private void CalculateXVelocity(float currentInput)
    {
        if (xForceSetThisFrame) return;
        if (!collision.Grounded)
        {
            //Die Reibung in der Luft im aktuellen Frame
            float currentDrag = maxSpeed * Time.fixedDeltaTime * airDragMultiplier
                * ((Mathf.Sign(currentInput) == Mathf.Sign(velocity.x)) ? 0.4f : 1f);

            //Wenden Reibung auf unsere aktuelle Geschwindigkeit (xVelocity) an.
            //Sollte unsere xVelocity kleiner als die abzuziehende Reibung sein, xVelocity = 0 (um nicht zu overshotten).
            velocity.x = (Mathf.Abs(velocity.x) <= currentDrag) ? 0f
                : velocity.x - (currentDrag * Mathf.Sign(velocity.x));

            //Addieren auf unsere xVelocity entsprechenden Input und Speed.
            velocity.x += Mathf.Clamp(value: currentInput * maxSpeed * Time.fixedDeltaTime * airControlMultiplier,
                min: -maxSpeed - Mathf.Max(-maxSpeed, velocity.x), //Stellen sicher, dass wir keine Geschwindigkeit addieren, falls
                max: maxSpeed - Mathf.Min(maxSpeed, velocity.x)); //bereits über maxSpeed sind.
        }
        else
        { //Aktuell einfach Präzise Movement für Boden
            velocity.x = currentInput * maxSpeed;
        }
    }

    private void ResetGravity()
    {
        velocity.y = 0;
    }

    protected void Jump()
    {
        SoundManager.Main.PlayNewSound(SoundType.playerJump);
        velocity.y = jumpForce;
        justJumped = true;
    }
    public void AddForce(Vector2 value) //Für später, wird noch nicht gebraucht
    {
        velocity += value;
    }
    public void SetForce(Vector2 value)
    {
        SetXForce(value.x);
        SetYForce(value.y);
    }
    /// <summary>
    /// No gravity will be applied the frame this function is called
    /// </summary>
    /// <param name="value"></param>
    public void SetYForce(float value)
    {
        if (justJumped) return;
        yForceSetThisFrame = true;
        velocity.y = value;
    }
    public void SetXForce(float value)
    {
        xForceSetThisFrame = true;
        velocity.x = value;
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, rayLength, groundLayer);

            if (debugRays)
                Debug.DrawRay(new Vector3(origin.x, origin.y, 0f), direction * rayLength, Color.red, .1f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == col || hit.collider.isTrigger) continue;

                hasHit = true;
            }
        }
        return hasHit;
    }

    private void OnDestroy()
    {
        collision.OnLandedEvent -= ResetGravity;
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
