using UnityEngine;

public class PlayerState_Climbing : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float jumpStrengthInMetersPerSecond = 2f;

    [SerializeField, Range(0, 90)] private float maxHorizontalAngleInDegrees = 30f;
    [SerializeField] private float handsDistance = 0.4f;
    [SerializeField] private float handsHeight = 1.5f;

    public float MaxHorizontalAngle_InDegrees => maxHorizontalAngleInDegrees;
    public float HandsReach => Mathf.Sin(maxHorizontalAngleInDegrees * Mathf.Deg2Rad);

    public override void Initialize()
    {
        BindInputUpdate(player.Input.Movement, HandleMovement);
        BindInputStart(player.Input.Jump, HandleJump);
    }
    protected override void EnterState()
    {

    }
    protected override void ExitState()
    {

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
        Vector3 handLeftPosition = player.GetInteractChecks(0).position;
        Vector3 handRightPosition = player.GetInteractChecks(1).position;

        Ray handLeftGrip = new(handLeftPosition, player.Forward);
        Ray handRightGrip = new(handRightPosition, player.Forward);

        bool leftHandHit = Physics.Raycast(handLeftGrip, out RaycastHit leftHandInfo, HandsReach);
        bool rightHandHit = Physics.Raycast(handRightGrip, out RaycastHit rightHandInfo, HandsReach);

        Vector3 grip = player.Forward;

        if (leftHandHit && rightHandHit)
        {
            float angle = Mathf.Atan2((rightHandInfo.distance - leftHandInfo.distance), (handsDistance * 2)) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, angle / 2, 0f);
            grip = rotation * player.Forward;
        }

        player.Gravity = grip;
    }

    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
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

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {

    }
}
