using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


[CreateAssetMenu(fileName = nameof(PlayerState_Grounded), menuName = "Scriptable Objects/" + nameof(PlayerState) + "/" + nameof(PlayerState_Grounded))]
public class PlayerState_Grounded : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float jumpStrengthInMetersPerSecond = 5f;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;

    protected override void EnterState()
    {
        player.Actions.Move.performed += HandleMovement_InputAction;
        player.Actions.Move.canceled += HandleMovement_InputAction;

        player.Actions.Jump.performed += HandleJump_InputAction;

        player.collisionUpdate += HandleCollisionUpdate;

        HandleGravity();
    }

    protected override void ExitState()
    {
        player.Actions.Move.performed -= HandleMovement_InputAction;
        player.Actions.Move.canceled -= HandleMovement_InputAction;

        player.Actions.Jump.performed -= HandleJump_InputAction;

        player.collisionUpdate -= HandleCollisionUpdate;
    }


    private void HandleMovement_InputAction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        HandleMovement(input);
    }
    private void HandleJump_InputAction(InputAction.CallbackContext context)
    {
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
        if (player.Forward == Vector3.zero)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            player.Look(forward);
        }

        Vector2 movementVelocity = input * movementSpeedInMetersPerSecond;
        player.Movement = movementVelocity;
    }

    private void HandleGravity()
    {
        float gravityForce = movementSpeedInMetersPerSecond / Mathf.Tan(player.Slope);

        Vector3 gravityVelocity = gravityDirection * gravityForce;
        player.Gravity = gravityVelocity;
    }

    private void HandleJump()
    {
        Vector3 gravityVelocity = gravityDirection * -jumpStrengthInMetersPerSecond;
        player.Gravity = gravityVelocity;

        player.SwitchState<PlayerState_Airbound>();
    }

    public override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        Quaternion rotation = Quaternion.LookRotation(forward);

        Vector3 velocityBuffer = new()
        {
            x = movement.x,
            z = movement.y,
        };
        velocityBuffer = rotation * velocityBuffer;

        if (movement != Vector2.zero)
        { player.Look(velocityBuffer); }

        velocityBuffer += rotation * gravity;

        Vector3 velocity = velocityBuffer;
        return velocity;
    }
}
