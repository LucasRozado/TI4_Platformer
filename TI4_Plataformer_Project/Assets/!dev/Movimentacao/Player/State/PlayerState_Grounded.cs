using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = nameof(PlayerState_Grounded), menuName = "Scriptable Objects/" + nameof(PlayerState) + "/" + nameof(PlayerState_Grounded))]
public class PlayerState_Grounded : PlayerState
{
    [SerializeField] private float movementSpeed_InMetersPerSecond = 5f;
    [SerializeField] private float jumpStrength_InMetersPerSecond = 2f;

    public override void Enter()
    {
        player.Actions.Move.performed += HandlePlayerMovement_MoveCallback;
        player.Actions.Move.canceled += HandlePlayerMovement_MoveCallback;

        player.Actions.Jump.performed += SetPlayerState_JumpCallback;
    }

    public override void Exit()
    {
        player.Actions.Move.performed -= HandlePlayerMovement_MoveCallback;
        player.Actions.Move.canceled -= HandlePlayerMovement_MoveCallback;

        player.Actions.Jump.performed -= SetPlayerState_JumpCallback;
    }


    private void HandlePlayerMovement_MoveCallback(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 velocity = GetVelocity(input);

        player.SetVelocity(velocity);
        player.SetForward(velocity);
    }

    private void SetPlayerState_JumpCallback(InputAction.CallbackContext context)
    {
        Vector3 jumpVelocity = new(0, jumpStrength_InMetersPerSecond, 0);
        player.AddVelocity(jumpVelocity);

        player.SwitchState<PlayerState_Airbound>();
    }


    private Vector3 GetVelocity(Vector2 input)
    {
        Vector3 velocity = HandleMovement(input);
        return velocity;
    }

    private bool isIdle;
    private Vector3 forward;
    private Vector3 HandleMovement(Vector2 input)
    {
        Vector3 relativeDirection = new(input.x, 0, input.y);
        Vector3 relativeVelocity = relativeDirection * movementSpeed_InMetersPerSecond;

        if (isIdle)
        {
            Vector3 forward = Camera.current.transform.forward;
            forward.y = 0;
            this.forward = forward;
        }
        else if (input == Vector2.zero)
        { isIdle = true; }

        Vector3 movementVelocity = Quaternion.LookRotation(forward) * relativeVelocity;
        Vector3 gravityVelocity = Physics.gravity * Time.deltaTime;

        Vector3 velocity = movementVelocity + gravityVelocity;
        return velocity;
    }
}
