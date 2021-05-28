using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : CharacterController2D, IDamageable
{
    private Vector2 moveInput = Vector2.zero;


    public CharacterAbility fireAbility;


    [SerializeField]
    private int maxJumps = 3;
    private int currentJumps = 0;

    [Header("Walljump")]
    [SerializeField]
    private float justWallJumpedTime = 1f;
    private float _justWallJumpedTime = 0;
    private bool justWallJumped
    {
        get => _justWallJumpedTime > 0;
    }
    [SerializeField]
    private float wallJumpForce = 4f;
    private float _wallJumpForce = 4f;
    [SerializeField]
    private float wallSlideSpeed = 3f;

    protected override void Start()
    {
        base.Start();
        collision.OnLandedEvent += OnLanded;
        collision.OnGroundLeftEvent += OnGroundLeft;

        fireAbility.Setup(this);
        currentJumps = maxJumps;
    }

    protected override void Update()
    {
        base.Update();
        fireAbility.Update();

        if(justWallJumped) _justWallJumpedTime -= Time.deltaTime;
    }

    private void OnGroundLeft()
    {
        currentJumps--;
    }
    private void OnLanded()
    {
        Debug.Log($"landed");
        currentJumps = maxJumps;
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleWallSlide();
    }
    private void HandleMove()
    {
        float moveXdelta = moveInput.x;

        float moveModifier = justWallJumped ? _wallJumpForce : 0f;

        if(moveModifier != 0f)
        {
            moveXdelta *= .5f;
        }

        Move(moveXdelta + moveModifier);
    }

    private void HandleWallSlide()
    {
        if(!collision.Grounded && (collision.leftCollision || collision.rightCollision) && rigid.velocity.y <= 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -wallSlideSpeed);
            currentJumps = maxJumps - 1;
            rigid.gravityScale = 0;
        } else
        {
            rigid.gravityScale = 3;
        }
    }

    private void HandleJump()
    {
        if (!collision.Grounded) //In der Luft
        {
            if (collision.rightCollision || collision.leftCollision)
            {
                currentJumps = maxJumps;
                _justWallJumpedTime = justWallJumpedTime;
                _wallJumpForce = collision.rightCollision ? -wallJumpForce : wallJumpForce;
            }
            else if (!(currentJumps > 0))
            {
                return;
            }
            currentJumps--;
        }

        Jump();
    }

    public void TakeDamage(int damage)
    {

    }

    #region Input

    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void DashInput(InputAction.CallbackContext context)
    {
        if (GameManager.Main.GameState != GameState.Playing) return;
        if(context.performed) //= GetKeyDown
        {
            
        }
        if(context.canceled) //GetKeyUp
        {
            
        }
    }
    public void FireInput(InputAction.CallbackContext context)
    {
        if (GameManager.Main.GameState != GameState.Playing) return;

        if (context.performed) //= GetKeyDown
        {
            fireAbility.OnAbilityButtonDown();
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (GameManager.Main.GameState != GameState.Playing) return;
        if (context.performed) //= GetKeyDown
        {
            HandleJump();
        }
    }

    public void PauseInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (GameManager.Main.GameState == GameState.Playing) GameManager.Main.ChangeGameState(GameState.Pause);
            else if (GameManager.Main.GameState == GameState.Pause) GameManager.Main.ChangeGameState(GameState.Playing);
        }
    }

    #endregion

    private void OnDisable()
    {
        collision.OnLandedEvent -= OnLanded;
        collision.OnGroundLeftEvent -= OnGroundLeft;
    }
}
