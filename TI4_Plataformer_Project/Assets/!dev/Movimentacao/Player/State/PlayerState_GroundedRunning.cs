using UnityEngine;
using System.Collections;

public class PlayerState_GroundedRunning : PlayerState
{
    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float jumpStrengthInMetersPerSecond = 5f;

    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float accelerationRate = 1f;
    [SerializeField] private float maxSpeed = 3f;

    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isInputing = false;
    
    [SerializeField] private Vector2 inputDirection;
    [SerializeField] private Vector2 lastInput;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;


    public override void Initialize()
    {
        BindInputUpdate(player.Input.Sprint, HandleSprint);

        BindInputUpdate(player.Input.Movement, HandleMovement);

        BindInputStart(player.Input.Jump, HandleJump);
    }
    protected override void EnterState()
    {
        HandleGravity();
    }
    protected override void ExitState()
    {

    }
   private void FixedUpdate()
    {
        player.Animator.SetFloat("sprintVelocity", speedMultiplier / 2);
        if(player.Velocity.x != 0f || player.Velocity.z != 0f)
        {
            player.Animator.SetBool("isSprintingIdle", false);
            player.Animator.SetBool("isSprinting", true);
        }
        else
        {
            player.Animator.SetBool("isSprintingIdle", true);
            player.Animator.SetBool("isSprinting", false);
        }

        if (speedMultiplier <= 1f)
        {
            lastInput = Vector2.zero;
        }
        if (isSprinting)
        {
            if (isInputing)
            {
                Vector2 movementVelocity =  speedMultiplier * inputDirection * movementSpeedInMetersPerSecond;
                player.Movement = movementVelocity;
                speedMultiplier += Time.fixedDeltaTime * accelerationRate;
                if (speedMultiplier > maxSpeed) { speedMultiplier = maxSpeed; }

            }
            else
            {
                Vector2 movementVelocity =  speedMultiplier * lastInput * movementSpeedInMetersPerSecond;
                player.Movement = movementVelocity;
                speedMultiplier -= Time.fixedDeltaTime * accelerationRate;
                if (speedMultiplier < 1f) { speedMultiplier = 1f; }
            }
        }
        else
        {
            HandleStop();
        }
    } 
    private void HandleStop()
    {
        speedMultiplier -= Time.fixedDeltaTime * accelerationRate;
        if (speedMultiplier <= 1f) 
        { 
            player.Animator.SetBool("isSprintingIdle", false);
            player.Animator.SetBool("isSprinting", false);
            player.Animator.SetBool("isIdleGround", true);

            speedMultiplier = 1f; lastInput = Vector2.zero; inputDirection = Vector2.zero; 
            player.SwitchState<PlayerState_Grounded>();
        }
        if (isInputing)
        {
            Vector2 movementVelocity =  speedMultiplier * inputDirection * movementSpeedInMetersPerSecond;
            player.Movement = movementVelocity;
        }
        else
        {
            Vector2 movementVelocity =  speedMultiplier * lastInput * movementSpeedInMetersPerSecond;
            player.Movement = movementVelocity;
        }
    }
    private void HandleSprint(float input)
    {
        Debug.Log(input);
        if (input >= 1f)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }
    private void HandleMovement(Vector2 input)
    {
        inputDirection = input;
        if (input.x != 0 || input.y != 0)
        {
            lastInput = input;
            isInputing = true;
        }
        else
        {
            isInputing = false;
        }
    }
    private void HandleJump()
    {
        player.Animator.SetTrigger("jump");

        Vector3 gravityVelocity = gravityDirection * -jumpStrengthInMetersPerSecond;
        player.Gravity = gravityVelocity;

        player.SwitchState<PlayerState_Jump>();
    }

    private void HandleGravity()
    {
        float gravityForce = movementSpeedInMetersPerSecond / Mathf.Tan(player.Slope);

        Vector3 gravityVelocity = gravityDirection * gravityForce;
        player.Gravity = gravityVelocity;
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

        if (movement != Vector2.zero)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            player.Forward = cameraForward;
            player.Look(velocityBuffer);
        }

        velocityBuffer += rotation * gravity;

        Vector3 velocity = velocityBuffer;
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
