using UnityEngine;
using System.Collections;

public class PlayerState_GroundedRunning : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float accelerationRate = 1f;
    [SerializeField] private float maxSpeed = 3f;

    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isInputing = false;
    
    [SerializeField] private Vector2 inputDirection;
    [SerializeField] private Vector2 lastInput;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;

    [SerializeField, Tooltip("In meters per second")]
    private float groundPull;
    private PlayerState_Jump jump;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_Jump>();

        BindInputUpdate(player.Input.Sprint, HandleSprint);
        BindInputUpdate(player.Input.Movement, HandleMovement);
        BindInputStart(player.Input.Jump, HandleJump);
    }
    protected override void EnterState()
    {
        player.Animator.SetBool("isWalking", false);
        player.Animator.SetBool("isIdleGround", false);

        if (jump.IsBuffered)
        { HandleJump(); }
        player.Animator.SetTrigger("fall");
        speedMultiplier = 1f;
    }
    protected override void ExitState()
    {
        isInputing = false;
        isSprinting = false;
        speedMultiplier = 1f;
    }
    
    private void Update()
    {
        UpdateMovement();
        UpdateGroundPull();

        RotatePlayer();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity, onCollision: HandleCollision);
    }
    private void UpdateGroundPull()
    {
        float groundPull = movementSpeed / Mathf.Tan(-player.Slope);
        this.groundPull = groundPull;
    }
    private void HandleStop()
    {
        Vector2 directionalInput = Vector2.zero;
        speedMultiplier -= Time.fixedDeltaTime * accelerationRate;
  
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        if (isInputing)
        {
            directionalInput =  speedMultiplier * player.Input.Movement.Value;
            //player.Movement = movementVelocity;
        }
        else
        {
            directionalInput =  speedMultiplier * lastInput;
            //player.Movement = movementVelocity;
        }
        if (speedMultiplier <= 1f) 
        { 
            speedMultiplier = 1f; lastInput = Vector2.zero; inputDirection = Vector2.zero; 
            if(!isSprinting) {player.SwitchState<PlayerState_Grounded>();}
        }
        Vector2 movementVelocity = rotation * (directionalInput * movementSpeed);
        directionalVelocity = movementVelocity;
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
    private void UpdateMovement()
    {
        Vector2 directionalInput = Vector2.zero;

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        player.Animator.SetFloat("sprintVelocity", speedMultiplier);

        if (speedMultiplier <= 1f)
        {
            lastInput = Vector2.zero;
        }
        if (isSprinting)
        {
            if (isInputing)
            {
                directionalInput =  speedMultiplier * player.Input.Movement.Value;
                lastInput = directionalInput.normalized;
                //player.Movement = movementVelocity;
                speedMultiplier += Time.fixedDeltaTime * accelerationRate;
                if (speedMultiplier > maxSpeed) { speedMultiplier = maxSpeed; }
            }
            else
            {
                directionalInput =  speedMultiplier * lastInput;
                //player.Movement = movementVelocity;
                HandleStop();
            }
        }
        else
        {
            if (isInputing)
            {
                directionalInput =  speedMultiplier * player.Input.Movement.Value;
                lastInput = directionalInput.normalized;
                //player.Movement = movementVelocity;
                HandleStop();
            }
            else
            {
                directionalInput =  speedMultiplier * lastInput;
                //player.Movement = movementVelocity;
                HandleStop();
            }
            HandleStop();
        }
        Vector2 movementVelocity = rotation * (directionalInput * movementSpeed);
        directionalVelocity = movementVelocity;
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
        float lastVelocity = speedMultiplier;
        PlayerState_RunningJump runningJump = player.GetState<PlayerState_RunningJump>();
        runningJump.SetLastVelocity(lastVelocity);
        player.SwitchState(runningJump);
    }
    /*
    private void HandleGravity()
    {
        float gravityForce = movementSpeed / Mathf.Tan(player.Slope);

        Vector3 gravityVelocity = gravityDirection * gravityForce;
        player.Gravity = gravityVelocity;
    }
    */
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
    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        throw new System.NotImplementedException();
    }

    protected void HandleCollision(CollisionFlags flags, ControllerColliderHit hit)
    {
        if (!flags.HasFlag(CollisionFlags.Below))
        {
            player.Move(new(0, -groundPull, 0));
            jump.StartCoyoteTimer();
            player.SwitchState<PlayerState_Airbound>();
            return;
        }

        else if (hit.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            player.SwitchState<PlayerState_Swim>();
            return;
        }
    }
    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        throw new System.NotImplementedException();
    }
}
