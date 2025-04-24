using UnityEngine;

public class PlayerState_Jump : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;

    [SerializeField, Tooltip("In seconds")]
    private float defaultTimeToApex = 0.4f;

    [SerializeField, Tooltip("In meters")]
    private float defaultHeight = 1.1f;

    public override void Initialize()
    {
        BindInputCancel(player.Input.Jump, CancelJump);
    }

    [Header("Observables")]

    [Tooltip("In meters per second")]
    private float initialJumpVelocity;

    [Tooltip("In meters per second per second")]
    private float gravityAcceleration;

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [SerializeField, Tooltip("In meters per second")]
    private float verticalVelocity;

    protected override void EnterState()
    {
        CalculateJumpStrength();

        verticalVelocity = initialJumpVelocity;
    }

    private void CancelJump()
    {
        player.SwitchState<PlayerState_Airbound>();
    }

    private void CalculateJumpStrength()
    {
        initialJumpVelocity = (2f * defaultHeight) / defaultTimeToApex;
        gravityAcceleration = (2f * defaultHeight) / -Mathf.Pow(defaultTimeToApex, 2);
    }

    private void Update()
    {
        UpdateMovement();
        UpdateGravity();

        RotatePlayer();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity,
            onCollision: HandleCollision
        );
    }

    private void RotatePlayer()
    {
        if (directionalVelocity != Vector2.zero)
        {
            Vector3 lookDirection = new()
            {
                x = directionalVelocity.x,
                y = 0,
                z = directionalVelocity.y,
            };
            player.Look(lookDirection);
        }
    }

    private void UpdateMovement()
    {
        Vector2 directionalInput = player.Input.Directional;

        Vector2 movementVelocity = directionalInput * movementSpeed;
        directionalVelocity = movementVelocity;
    }

    private void UpdateGravity()
    {
        if (verticalVelocity > 0)
        {
            float velocityFromGravity = gravityAcceleration * Time.deltaTime;
            verticalVelocity += velocityFromGravity;

            if (verticalVelocity <= 0)
            { player.SwitchState<PlayerState_Airbound>(); }
        }
    }

    private Vector3 CalculateVelocity()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.LookRotation(cameraDirection); }
        else
        { rotation = Quaternion.identity; }

        Vector3 velocity = rotation * new Vector3()
        {
            x = directionalVelocity.x,
            y = verticalVelocity,
            z = directionalVelocity.y,
        };

        return velocity;
    }

    private void HandleCollision(CollisionFlags flags, ControllerColliderHit hit)
    {
        bool hitGround = flags.HasFlag(CollisionFlags.Below);
        if (hitGround)
        {
            player.SwitchState<PlayerState_Grounded>();
            return;
        }

        bool hitWall = flags.HasFlag(CollisionFlags.Sides);
        bool hitClimbable = hit.gameObject.CompareTag("CanClimb");
        if (hitWall && hitClimbable)
        {
            float angle = player.GetState<PlayerState_Climbing>().MaxHorizontalAngle_InDegrees;
            // Comparando o ângulo entre a frente do jogador e a normal da parede
            if (Mathf.Abs(Vector3.Dot(player.Forward, hit.normal)) > Mathf.Cos(angle * Mathf.Deg2Rad))
            {
                player.Look(-hit.normal);
                player.SwitchState<PlayerState_Climbing>();
            }
            return;
        }
    }

    protected override void ExitState()
    {

    }

    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    { throw new System.NotImplementedException(); }
    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    { throw new System.NotImplementedException(); }
}
