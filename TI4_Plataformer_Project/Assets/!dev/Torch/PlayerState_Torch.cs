using UnityEngine;
using Unity.Cinemachine;

public class PlayerState_Torch : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 2.5f;

    public float EnemyDistance => enemyDistance;
    [SerializeField, Tooltip("In meters")]
    private float enemyDistance = 3f;

    [SerializeField, Tooltip("In seconds")]
    private float cooldown = 5f;

    [SerializeField] private CinemachineCamera forwardCamera;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [SerializeField, Tooltip("In meters per second")]
    private float groundPull;

    public override void Initialize()
    {

    }

    protected override void EnterState()
    {

    }

    private void Update()
    {
        UpdateMovement();
        UpdateGroundPull();

        RotatePlayer();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity, onCollision: HandleCollision);
    }

    private void UpdateMovement()
    {
        Vector2 directionalInput = player.Input.Movement.Value;

        Vector3 cameraDirection = forwardCamera.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -forwardCamera.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        if (directionalInput != Vector2.zero)
        {
            // TODO: Walk Animation
        }
        else
        {
            // TODO: Idle Animation
        }

        Vector2 movementVelocity = rotation * (directionalInput * movementSpeed);
        directionalVelocity = movementVelocity;
    }

    private void UpdateGroundPull()
    {
        float groundPull = movementSpeed / Mathf.Tan(-player.Slope);
        this.groundPull = groundPull;
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

    private Vector3 CalculateVelocity()
    {
        Vector3 velocity = new()
        {
            x = directionalVelocity.x,
            y = groundPull,
            z = directionalVelocity.y,
        };

        return velocity;
    }

    protected void HandleCollision(CollisionFlags flags, ControllerColliderHit hit)
    {
        if (!flags.HasFlag(CollisionFlags.Below))
        {
            player.Move(new(0, -groundPull, 0));
            player.SwitchState<PlayerState_Airbound>();
            player.GetState<PlayerState_Jump>().StartCoyoteTimer();
            return;
        }

        else if (hit.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            player.SwitchState<PlayerState_Swim>();
            return;
        }
    }

    protected override void ExitState()
    {

    }

    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        throw new System.NotImplementedException();
    }

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        throw new System.NotImplementedException();
    }
}
