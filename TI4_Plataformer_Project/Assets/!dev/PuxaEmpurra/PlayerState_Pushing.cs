using UnityEngine;

public class PlayerState_Pushing : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 4f;
    private PushableObject pushable;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;

    public override void Initialize()
    {
        BindInputUpdate(player.Input.Movement, HandleMovement);
        BindInputStart(player.Input.Interact, HandleInteraction);
    }
    protected override void EnterState()
    {
        HandleObject();
        HandleGravity();
    }
    protected override void ExitState()
    {
        pushable.transform.parent = null;
    }

    private void HandleMovement(Vector2 input)
    {
        input.x = 0;
        
        Vector2 movementVelocity = input * movementSpeedInMetersPerSecond;
        player.Movement = movementVelocity;
    }

    private void HandleInteraction()
    {
        player.SwitchState<PlayerState_Grounded>();
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

    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
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

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        if (!collision.flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Airbound>();
        }
    }
}
