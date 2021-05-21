using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D col;

    public delegate void ColEvent();
    public ColEvent OnLandedEvent;
    public ColEvent OnGroundLeftEvent;

    [SerializeField]
    private int groundRays = 6;
    [SerializeField]
    private float rayLength = .01f;

    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    protected float jumpForce = 12f;

    [SerializeField]
    private bool debugRays = false;

    protected bool Grounded { get => grounded; set
        {
            if(grounded != value)
            {
                grounded = value;
                if(grounded)
                {
                    OnLandedEvent?.Invoke();
                } else
                {
                    OnGroundLeftEvent?.Invoke();
                }
            }
        } 
    }
    private bool grounded = false;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        CheckGrounded();
    }

    protected void Move(float horizontalInput)
    {
        rigid.velocity = new Vector2(horizontalInput * speed, rigid.velocity.y);
    }
    protected void Jump()
    {
        SoundManager.Main.PlayNewSound(SoundType.playerJump);
        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
    }

    private void CheckGrounded()
    {
        Vector2 bottomLeft = col.bounds.center + new Vector3(-col.bounds.extents.x, -col.bounds.extents.y, 0f);
        Vector2 bottomRight = col.bounds.center + new Vector3(col.bounds.extents.x, -col.bounds.extents.y, 0f);

        bool hasHit = false;

        for(int i = 0; i < groundRays; i++)
        {
            if (hasHit) break;

            Vector2 origin = bottomLeft + i * ((bottomRight - bottomLeft) / (groundRays - 1));
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.down, rayLength);

            if(debugRays)
                Debug.DrawRay(new Vector3(origin.x, origin.y, 0f), Vector3.down * rayLength, Color.red, .1f);

            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider == col) continue;

                hasHit = true;
            }
        }
        Grounded = hasHit;
    }
}
