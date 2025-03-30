using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = nameof(PlayerState_Climbing), menuName = "Scriptable Objects/" + nameof(PlayerState) + "/" + nameof(PlayerState_Climbing))]
public class PlayerState_Climbing : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float jumpStrengthInMetersPerSecond = 2f;

    [SerializeField, Range(0, 90)] private float maxHorizontalAngleInDegrees = 30f;
    [SerializeField] private float handsDistance = 0.4f;
    [SerializeField] private float handsHeight = 1.5f;

    public float MaxHorizontalAngle_InDegrees => maxHorizontalAngleInDegrees;
    public float HandsReach => Mathf.Sin(maxHorizontalAngleInDegrees * Mathf.Deg2Rad);


    protected override void EnterState()
    {
        player.Actions.Move.performed += HandleMovement_InputAction;
        player.Actions.Move.canceled += HandleMovement_InputAction;

        player.Actions.Jump.performed += HandleJump_InputAction;
    }

    protected override void ExitState()
    {
        player.Actions.Move.performed -= HandleMovement_InputAction;
        player.Actions.Move.canceled -= HandleMovement_InputAction;

        player.Actions.Jump.performed -= HandleJump_InputAction;
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


    private void HandleMovement(Vector2 input)
    {
        player.Movement = input * movementSpeedInMetersPerSecond;
        HandleGrip();
    }

private void HandleJump()
    {
        player.Gravity = -player.Gravity;

        player.SwitchState<PlayerState_Airbound>();
    }

    private void HandleGrip()
    {
        Vector3 up = Quaternion.LookRotation(Vector3.up) * player.Forward;
        Vector3 right = Quaternion.LookRotation(Vector3.right) * player.Forward;

        Vector3 handsCenterPosition = player.transform.position + up * handsHeight;
        Vector3 handLeftPosition = handsCenterPosition - right * handsDistance;
        Vector3 handRightPosition = handsCenterPosition + right * handsDistance;

        Ray handLeftGrip = new(handLeftPosition, player.Forward);
        Ray handRightGrip = new(handRightPosition, player.Forward);

        bool leftHandHit = Physics.Raycast(handLeftGrip, out RaycastHit leftHandInfo, HandsReach);
        bool rightHandHit = Physics.Raycast(handLeftGrip, out RaycastHit rightHandInfo, HandsReach);

        Vector3 grip = player.Forward;

        if (leftHandHit && rightHandHit)
        {
            float angle = Mathf.Atan2((rightHandInfo.distance - leftHandInfo.distance), (handsDistance * 2)) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, angle / 2, 0f);
            grip = rotation * player.Forward;
        }

        player.Gravity = grip;
    }

    public override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        Quaternion rotation = Quaternion.LookRotation(gravity);

        Vector3 velocityBuffer = new()
        {
            x = movement.x,
            y = movement.y,
        };
        velocityBuffer += gravity;
        velocityBuffer = rotation * velocityBuffer;

        Vector3 velocity = velocityBuffer;
        return velocity;
    }
}
