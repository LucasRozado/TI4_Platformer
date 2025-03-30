using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = nameof(PlayerState_Pushing), menuName = "Scriptable Objects/" + nameof(PlayerState) + "/" + nameof(PlayerState_Pushing))]

public class PlayerState_Pushing : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 4f;
    private PushableObject pushable;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;

    protected override void EnterState()
    {
        player.Actions.Move.performed += HandleMovement_InputAction;
        player.Actions.Move.canceled += HandleMovement_InputAction;

        player.Actions.Interact.performed += HandleInteraction;

        player.collisionUpdate += HandleCollisionUpdate;


        HandleObject();
        HandleGravity();
    }

    protected override void ExitState()
    {
        player.Actions.Move.performed -= HandleMovement_InputAction;
        player.Actions.Move.canceled -= HandleMovement_InputAction;

        player.Actions.Interact.performed -= HandleInteraction;

        player.collisionUpdate -= HandleCollisionUpdate;

        pushable.transform.parent = null;
    }
    private void HandleMovement_InputAction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        HandleMovement(input);
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
        input.x = 0;
        
        Vector2 movementVelocity = input * movementSpeedInMetersPerSecond;
        player.Movement = movementVelocity;
    }

    private void HandleGravity()
    {
        float gravityForce = movementSpeedInMetersPerSecond / Mathf.Tan(player.Slope);

        Vector3 gravityVelocity = gravityDirection * gravityForce;
        player.Gravity = gravityVelocity;
    }

    public void HandleObject()
    {
        RaycastHit hit;
        Vector3 playerCenter = (player.GetInteractChecks(0).position - player.GetInteractChecks(1).position) / 2;
        playerCenter += player.GetInteractChecks(1).position;
        Physics.Raycast(playerCenter, player.Forward, out hit, player.InteractDistance, player.CanInteract);
        player.Forward = -hit.normal;
        pushable = hit.collider.gameObject.GetComponent<PushableObject>();
        pushable.transform.parent = player.transform;
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

        velocityBuffer += rotation * gravity;

        Vector3 velocity = velocityBuffer;

        if (pushable.CheckCollision(player) && movement.y > 0)
        {
            velocity = Vector2.zero;
        }

        return velocity;
    }

    private void HandleInteraction(InputAction.CallbackContext context)
    {
        player.SwitchState<PlayerState_Grounded>();
    }
}
