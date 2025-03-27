using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = nameof(PlayerState_Airbound), menuName = "Scriptable Objects/" + nameof(PlayerState) + "/" + nameof(PlayerState_Airbound))]
public class PlayerState_Airbound : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float rotationSpeedInDegreesPerSeconds = 270f;
    [SerializeField] private float terminalVelocityInMetersPerSecond = 10f;


    public override void Enter()
    {
        player.Actions.Move.performed += HandlePlayerMovement_MoveCallback;
        player.Actions.Move.canceled += HandlePlayerMovement_MoveCallback;

        player.collided += SetPlayerState_CollisionCallback;
    }

    public override void Exit()
    {
        player.Actions.Move.performed -= HandlePlayerMovement_MoveCallback;
        player.Actions.Move.canceled -= HandlePlayerMovement_MoveCallback;

        player.collided -= SetPlayerState_CollisionCallback;
    }


    private void HandlePlayerMovement_MoveCallback(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 velocity = GetVelocity(input);

        player.SetVelocity(velocity);
        player.SetForward(velocity);
    }

    private void SetPlayerState_CollisionCallback(ControllerColliderHit hit, CollisionFlags flags)
    {
        if (flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Grounded>();
        }
        else if (hit.gameObject.CompareTag("CanClimb"))
        {
            float angle = player.GetState<PlayerState_Climbing>().MaxHorizontalAngle_InDegrees;
            // Comparando o ângulo entre a frente do jogador e a normal da parede
            if (Mathf.Abs(Vector3.Dot(player.Forward, hit.normal)) > Mathf.Cos(angle * Mathf.Deg2Rad))
            {
                player.SetForward(-hit.normal);
                player.SwitchState<PlayerState_Climbing>();
            }
        }
    }


    private Vector3 GetVelocity(Vector2 input)
    {
        Vector3 movementVelocity = HandleMovement(input);
        Vector3 gravityVelocity = HandleGravity();
        Vector3 velocity = movementVelocity + gravityVelocity;
        return velocity;
    }

    private bool isIdle;
    private Vector3 forward;
    private Vector3 HandleMovement(Vector2 input)
    {
        Vector3 relativeDirection = new(input.x, 0, input.y);
        Vector3 relativeVelocity = relativeDirection * movementSpeedInMetersPerSecond;

        if (isIdle)
        {
            Vector3 forward = Camera.current.transform.forward;
            forward.y = 0;
            this.forward = forward;
        }
        else if (input == Vector2.zero)
        { isIdle = true; }

        Vector3 movementVelocity = Quaternion.LookRotation(forward) * relativeVelocity;
        return movementVelocity;
    }

    private Vector3 HandleGravity()
    {
        Vector3 currentGravityVelocity = Vector3.Dot(player.Velocity, Physics.gravity) * Physics.gravity.normalized;
        Vector3 gravityVelocity = currentGravityVelocity + Physics.gravity * Time.deltaTime;

        // Terminal velocity
        if (gravityVelocity.magnitude > terminalVelocityInMetersPerSecond)
        { gravityVelocity = Physics.gravity.normalized * terminalVelocityInMetersPerSecond; }

        return gravityVelocity;
    }
}