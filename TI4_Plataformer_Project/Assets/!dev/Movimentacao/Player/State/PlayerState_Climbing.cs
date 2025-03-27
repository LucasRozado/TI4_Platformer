using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = nameof(PlayerState_Climbing), menuName = "Scriptable Objects/" + nameof(PlayerState) + "/" + nameof(PlayerState_Climbing))]
public class PlayerState_Climbing : PlayerState
{
    [SerializeField] private float movementSpeed_InMetersPerSecond = 5f;
    [SerializeField] private float jumpStrength_InMetersPerSecond = 2f;

    [SerializeField, Range(0, 90)] private float maxHorizontalAngle_InDegrees = 30f;
    [SerializeField] private float handsDistance = 0.4f;
    [SerializeField] private float handsHeight = 1.5f;


    public float MaxHorizontalAngle_InDegrees => maxHorizontalAngle_InDegrees;
    public float HandsReach => Mathf.Sin(maxHorizontalAngle_InDegrees * Mathf.Deg2Rad);


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
        Vector3 jumpVelocity = HandleJump();
        player.AddVelocity(jumpVelocity);

        player.SwitchState<PlayerState_Airbound>();
    }


    private Vector3 GetVelocity(Vector2 input)
    {
        Vector3 gravityVelocity = HandleGravity();
        Vector3 movementVelocity = HandleMovement(input);
        Vector3 velocity = movementVelocity + gravityVelocity;
        return velocity;
    }

    private Vector3 forward;
    private Vector3 HandleGravity()
    {
        Vector3 up = Quaternion.LookRotation(Vector3.up) * forward;
        Vector3 right = Quaternion.LookRotation(Vector3.right) * forward;

        Vector3 handsCenterPosition = player.transform.position + up * handsHeight;
        Vector3 handLeftPosition = handsCenterPosition - right * handsDistance;
        Vector3 handRightPosition = handsCenterPosition + right * handsDistance;

        Ray handLeftGrip = new(handLeftPosition, forward);
        Ray handRightGrip = new(handRightPosition, forward);

        bool leftHandHit = Physics.Raycast(handLeftGrip, out RaycastHit leftHandInfo, HandsReach);
        bool rightHandHit = Physics.Raycast(handLeftGrip, out RaycastHit rightHandInfo, HandsReach);

        Vector3 velocity = forward;

        if (leftHandHit && rightHandHit)
        {
            float angle = Mathf.Atan2((rightHandInfo.distance - leftHandInfo.distance), (handsDistance * 2)) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, angle / 2, 0f);
            velocity = rotation * forward;
        }

        return velocity;
    }

    private Vector3 HandleMovement(Vector2 input)
    {
        Vector3 relativeDirection = new(input.x, input.y, 0);
        Vector3 relativeVelocity = relativeDirection * movementSpeed_InMetersPerSecond;

        Vector3 velocity = Quaternion.LookRotation(forward) * relativeVelocity;
        return velocity;
    }

    private Vector3 HandleJump()
    {
        Vector3 horizontalVelocity = -player.Forward * movementSpeed_InMetersPerSecond;
        Vector3 verticalVelocity = jumpStrength_InMetersPerSecond * Vector3.up;
        return verticalVelocity + horizontalVelocity;
    }
}
