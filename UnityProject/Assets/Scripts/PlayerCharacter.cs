using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : CharacterController2D
{
    private Vector2 moveInput = Vector2.zero;

    [SerializeField]
    private int jumps = 3;
    private int currentJumps = 3;

    protected override void Start()
    {
        base.Start();
        base.OnLandedEvent += OnLanded;
        OnGroundLeftEvent += OnGroundLeft;
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
}
