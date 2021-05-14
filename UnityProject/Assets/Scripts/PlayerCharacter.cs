using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : CharacterController2D, IDamageable
{
    private Vector2 moveInput = Vector2.zero;


    public CharacterAbility fireAbility;


    [SerializeField]
    private int jumps = 3;
    private int currentJumps = 3;

    protected override void Start()
    {
        base.Start();
        base.OnLandedEvent += OnLanded;
        OnGroundLeftEvent += OnGroundLeft;

        fireAbility.Setup(this);
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
        Debug.Log($"landed");
        currentJumps = jumps;
    }

    private void FixedUpdate()
    {
        Move(moveInput.x); 
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void DashInput(InputAction.CallbackContext context)
    {
        if(context.performed) //= GetKeyDown
        {

        }
        if(context.canceled) //GetKeyUp
        {
            
        }
    }
    public void FireInput(InputAction.CallbackContext context)
    {
        if (context.performed) //= GetKeyDown
        {
            fireAbility.OnAbilityButtonDown();
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentJumps > 0) //= GetKeyDown
        {
            if (!Grounded) currentJumps--;

            Jump();
        }
    }

    private void OnDisable()
    {
        base.OnLandedEvent -= OnLanded;
        OnGroundLeftEvent -= OnGroundLeft;
    }

    public void TakeDamage(int damage)
    {
       
    }
}
