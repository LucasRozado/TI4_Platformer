using UnityEngine;

public class PlayerState_Spin : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 4f;
    [SerializeField] private float rotationSpeed = 40f;
    float rotationDirection;
    private GameObject spinable;
    private readonly Vector3 gravityDirection = Physics.gravity.normalized;
    public override void Initialize()
    {
        BindInputStart(player.Input.Interact, HandleInteraction);
        BindInputUpdate(player.Input.Movement, HandleMovement);
    }

    public void HandleInteraction()
    {
        player.SwitchState<PlayerState_Grounded>();
    }

    public void HandleMovement(Vector2 input)
    {
        rotationDirection = input.y * rotationSpeed;
    }
    public void HandleObject(GameObject spinable, float speed)
    {
        this.spinable = spinable;
        rotationSpeed = speed;
        player.transform.parent = spinable.transform;
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

        return velocity;
    }

    protected override void EnterState()
    {
        HandleGravity();        
    }

    protected override void ExitState()
    {
        player.transform.parent = null;
    }

    private void HandleGravity()
    {
        float gravityForce = movementSpeedInMetersPerSecond / Mathf.Tan(player.Slope);

        Vector3 gravityVelocity = gravityDirection * gravityForce;
        player.Gravity = gravityVelocity;
    }

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        if (!collision.flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Airbound>();
        }
    }

    public void Update()
    {

        spinable.transform.Rotate(Vector3.up, rotationDirection * Time.deltaTime);
    }
}
