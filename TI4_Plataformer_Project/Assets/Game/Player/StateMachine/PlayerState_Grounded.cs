using UnityEngine;

public class PlayerState_Grounded : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float jumpStrengthInMetersPerSecond = 5f;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;

    public override void Initialize()
    {
        BindInputUpdate(player.Input.Movement, HandleMovement);

        BindInputStart(player.Input.Jump, HandleJump);
        BindInputStart(player.Input.Interact, HandleInteraction);
    }
    protected override void EnterState()
    {
        player.animator.SetBool("isAirBourne", false);
        if (player.Velocity.x != 0 || player.Velocity.z != 0)
        {
            player.animator.SetBool("isWalking", true);
        }
        else
        {
            player.animator.SetBool("isIdleGround", true);
        }

        HandleGravity();
    }
    protected override void ExitState()
    {
        player.animator.SetBool("isIdleGround", false);
    }


    private void HandleMovement_InputAction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        HandleMovement(input);
    }
    private void HandleJump_InputAction(InputAction.CallbackContext context)
    {
        player.animator.SetTrigger("jump");
        HandleJump();
    }

    private void HandleCollisionUpdate(ControllerColliderHit hit, CollisionFlags flags)
    {
        if (!flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Airbound>();
        }
    }

    private void HandleMovement(Vector2 input)
    {
        Vector2 movementVelocity = input * movementSpeedInMetersPerSecond;
        if(movementVelocity == Vector2.zero)
        {
            player.animator.SetBool("isIdleGround", true);
            player.animator.SetBool("isWalking", false);
        }
        else
        {
            player.animator.SetBool("isIdleGround", false);
            player.animator.SetBool("isWalking", true);
        }
        player.Movement = movementVelocity;
    }

    private void HandleJump()
    {
        Debug.Log("Ground Jump");
        player.SwitchState<PlayerState_Jump>();
    }

    private void HandleInteraction()
    {
        Debug.Log("Interaction");
        Transform checkL = player.GetInteractChecks(0);
        Transform checkR = player.GetInteractChecks(1);
        RaycastHit hitL;
        RaycastHit hitR;

        Physics.Raycast(checkL.position, checkL.forward, out hitL, player.InteractDistance, player.CanInteract);
        Physics.Raycast(checkR.position, checkR.forward, out hitR, player.InteractDistance, player.CanInteract);

        Debug.Log(hitL.collider);
        Debug.Log(hitR.collider);
        if (hitL.collider != null && hitL.collider == hitR.collider)
        {
            Debug.Log("Target acquired");
            if (hitL.collider.TryGetComponent(out Interactable interactable))
            {
                interactable.InteractWith(player);
                Debug.Log("Interact Done");
            }
        }
    }

    private void HandleGravity()
    {
        float gravityForce = movementSpeedInMetersPerSecond / Mathf.Tan(player.Slope);

        Vector3 gravityVelocity = gravityDirection * gravityForce;
        player.Gravity = gravityVelocity;
    }

    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        Quaternion rotation = Quaternion.LookRotation(forward);

        Vector3 velocityBuffer = new()
        {
            x = movement.x,
            z = movement.y,
        };
        velocityBuffer = rotation * velocityBuffer;

        if (movement != Vector2.zero)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            player.Forward = cameraForward;
            player.Look(velocityBuffer);
        }

        velocityBuffer += rotation * gravity;

        Vector3 velocity = velocityBuffer;
        return velocity;
    }

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        if (!collision.flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Airbound>();
        }
    }
}
