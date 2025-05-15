using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class PlayerState_Airbound : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;

    [SerializeField, Tooltip("In meters per seconds")]
    private float terminalVelocity = 10f;

    [Header("Observables")]

    [Tooltip("In meters per second per second")]
    private float gravityAcceleration;

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [Tooltip("In meters per second")]
    public float verticalVelocity;

    [SerializeField]
    private bool isSprinting = false;
    [SerializeField] private CinemachineCamera forwardCamera;

    private PlayerState_Jump jump;
    public override void Initialize()
    {
        player.GetState(out jump);

        BindInputUpdate(player.Input.Sprint, HandleSprint);
        BindInputStart(player.Input.Jump, HandleCoyoteJump);
    }

    protected override void EnterState()
    {
        CalculateParameters();

        verticalVelocity = 0;

        player.Animator.SetBool("isAirBourne", true);
    }
    protected override void ExitState()
    {
        player.Animator.SetBool("isAirBourne", false);
        player.Animator.SetTrigger("land");
    }

    private void CalculateParameters()
    {
        gravityAcceleration = (2f * jump.DefaultHeight) / -Mathf.Pow(jump.DefaultFallTime, 2);
    }

    private void HandleSprint(float input)
    {
        if (input >= 1f)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void HandleCoyoteJump()
    {
        if (jump.IsOnCoyoteTime)
        { player.SwitchState(jump); }
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

        Vector2 movementVelocity = rotation * (directionalInput * movementSpeed);
        directionalVelocity = movementVelocity;
    }

    private void UpdateGravity()
    {
        if (verticalVelocity > -terminalVelocity)
        {
            float velocityFromGravity = gravityAcceleration * Time.deltaTime;
            verticalVelocity += velocityFromGravity;

            if (verticalVelocity < -terminalVelocity)
            { verticalVelocity = -terminalVelocity; }
        }
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
            if (hit.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                player.SwitchState<PlayerState_Swim>();
                return;
            }
            else if (hit.gameObject.TryGetComponent(out BoostMushroom boostMushroom))
            {
                boostMushroom.Boost(player);
                return;
            }
            else if (isSprinting)
            {
                player.SwitchState<PlayerState_GroundedRunning>();
            }
            else
            {
                if (player.Velocity.x != 0 || player.Velocity.z != 0)
                { player.Animator.SetBool("isWalking", true); player.Animator.SetBool("isIdleGround", false); }
                else
                { player.Animator.SetBool("isWalking", false); player.Animator.SetBool("isIdleGround", true); }

                player.SwitchState<PlayerState_Grounded>();
            }
            return;
        }

        bool hitWall = flags.HasFlag(CollisionFlags.Sides);
        if (hitWall && hit.gameObject.TryGetComponent<ClimbWall>(out _))
        {
            player.Look(-hit.normal);
            player.SwitchState<PlayerState_Climbing>();
        }
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