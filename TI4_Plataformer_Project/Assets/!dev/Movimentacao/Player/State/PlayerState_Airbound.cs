using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState_Airbound : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float rotationSpeedInDegreesPerSeconds = 270f;
    [SerializeField] private float terminalVelocityInMetersPerSecond = 10f;

    protected override void EnterState()
    {
        player.Actions.Move.performed += HandleMovement_InputAction;
        player.Actions.Move.canceled += HandleMovement_InputAction;

        player.collisionUpdate += HandleCollisionUpdate;

        HandleCoroutine(HandleGravity_Coroutine());
    }

    protected override void ExitState()
    {
        player.Actions.Move.performed -= HandleMovement_InputAction;
        player.Actions.Move.canceled -= HandleMovement_InputAction;

        player.collisionUpdate -= HandleCollisionUpdate;
    }

    private void HandleCollisionUpdate(ControllerColliderHit hit, CollisionFlags flags)
    {
        if (flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Grounded>();
        }
        else if (hit != null && hit.gameObject.CompareTag("CanClimb"))
        {
            float angle = player.GetState<PlayerState_Climbing>().MaxHorizontalAngle_InDegrees;
            // Comparando o ângulo entre a frente do jogador e a normal da parede
            if (Mathf.Abs(Vector3.Dot(player.Forward, hit.normal)) > Mathf.Cos(angle * Mathf.Deg2Rad))
            {
                player.Look(-hit.normal);
                player.SwitchState<PlayerState_Climbing>();
            }
        }
    }

    private void HandleMovement_InputAction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        HandleMovement(input);
    }

    private void HandleMovement(Vector2 input)
    {
        if (player.Forward == Vector3.zero)
        { HandleForward(); }

        Vector2 movementVelocity = input * movementSpeedInMetersPerSecond;
        player.Movement = movementVelocity;
    }

    private void HandleForward()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        player.Forward = forward;
    }

    private IEnumerator HandleGravity_Coroutine()
    {
        float gravityStrength = Physics.gravity.magnitude;
        Vector3 gravityDirection = Physics.gravity.normalized;

        while (true)
        {
            float currentGravity = Vector3.Dot(player.Gravity, gravityDirection);
            if (currentGravity < terminalVelocityInMetersPerSecond)
            {
                float gravityAcceleration = gravityStrength * Time.deltaTime;
                currentGravity += gravityAcceleration;
                if (currentGravity > terminalVelocityInMetersPerSecond)
                { currentGravity = terminalVelocityInMetersPerSecond; }

                player.Gravity = currentGravity * gravityDirection;
            }

            yield return null;
        }
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
        { player.Look(velocityBuffer); }

        velocityBuffer += rotation * gravity;

        Vector3 velocity = velocityBuffer;
        return velocity;
    }
}