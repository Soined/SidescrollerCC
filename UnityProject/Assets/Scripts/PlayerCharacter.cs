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
    private float wallJumpForce = 4f;
    [SerializeField]
    private float wallSlideSpeed = 3f;

    private IInteractable interactable;
    public IInteractable Interactable
    {
        get => interactable;
        set
        {
            if (interactable != null) interactable.OnCurrentLost();
            interactable = value;
            if (interactable != null) interactable.OnCurrent();
        }
    }

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
    }

    private void OnGroundLeft()
    {
        currentJumps--;
    }
    private void OnLanded()
    {
        currentJumps = maxJumps;
    }

    private void FixedUpdate()
    {
        HandleWallSlide();
        HandleMove();
    }
    private void HandleMove()
    {
        Move(moveInput.x);
    }

    private void HandleWallSlide()
    {
        if(!collision.Grounded && (collision.leftCollision || collision.rightCollision) && rigid.velocity.y <= 0.1f)
        {
            currentJumps = maxJumps - 1;
            SetYForce(-wallSlideSpeed);
        }
    }

    private void HandleJump()
    {
        if (!collision.Grounded) //In der Luft
        {
            if (collision.rightCollision || collision.leftCollision)
            {
                WallJump();
            }
            else if (!(currentJumps > 0))
            {
                return;
            }
            currentJumps--;
        }

        Jump();
    }
    private void WallJump()
    {
        currentJumps = maxJumps;
        //Wir setzen xVelocity im Controller auf wallJumpForce, bzw. -wallJumpForce, je nach dem, in welche Richtung wir walljumpen
        SetXForce((collision.rightCollision) ? -wallJumpForce : wallJumpForce);
    }

    public void TakeDamage(int damage)
    {

    }

    //TODO: Subscribe to GameManager OnGameStateChanged()

    #region GPInput

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
            if(Interactable != null)
            {
                Interactable.OnInteract();
                return;
            }

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
    #region UIInput
    public void OnSubmitButtonDown(InputAction.CallbackContext context)
    {
        UIManager.Main.OnSubmitButton();
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<IInteractable>() is IInteractable inter)
        {
            Interactable = inter;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            Interactable = null;
        }
    }

    private void OnDisable()
    {
        collision.OnLandedEvent -= OnLanded;
        collision.OnGroundLeftEvent -= OnGroundLeft;
    }
}
