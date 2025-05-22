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
        BindInputStart(player.Input.Spirit, SwitchReality);
    }
    protected override void EnterState()
    {
        player.PlayerAnimations.HoldAnimation(true);
        player.PlayerAnimations.HoldTypeAnimation(0);
        player.Forward = player.transform.forward;
        HandleGravity();
    }
    protected override void ExitState()
    {
        player.PlayerAnimations.HoldAnimation(false);
        player.PlayerAnimations.HoldTypeAnimation(0);
        pushable.transform.parent = null;
    }

    private void HandleMovement(Vector2 input)
    {
        if (input.y > 0)
        {
            player.PlayerAnimations.HoldTypeAnimation(1);
        }
        else if (input.y < 0)
        {
            player.PlayerAnimations.HoldTypeAnimation(-1);
        }
        else
        {
            player.PlayerAnimations.HoldTypeAnimation(0);
        }

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

    public void HandleObject(PushableObject pushable)
    {
        this.pushable = pushable;
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
