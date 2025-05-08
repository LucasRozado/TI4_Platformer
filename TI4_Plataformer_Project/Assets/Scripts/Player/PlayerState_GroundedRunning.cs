using UnityEngine;
using System.Collections;

public class PlayerState_GroundedRunning : PlayerState
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float turnSpeed = 5f;

    [SerializeField] private float movementSpeed = 5f;

    [Header("Values")]

    private Vector3 currentVelocity;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [Header("Physics")]
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float surfaceFloatForce = 2f;

    [SerializeField] private float speedMultiplier = 1f;

    [Header("Inputs")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isInputing = false;
    
    [SerializeField] private Vector3 inputDirection;
    [SerializeField] private Vector3 lastInput;

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

        //Vector3 velocity = CalculateVelocity();
        //player.Move(velocity, onCollision: HandleCollision);
    }
    private void UpdateGroundPull()
    {
        float groundPull =  movementSpeed / Mathf.Tan(-player.Slope);
        this.groundPull = groundPull;
    }
    private Vector3 HandleStop()
    {
        /*
        Vector2 directionalInput = Vector2.zero;
  
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        if (isInputing)
        {
            inputDirection =  speedMultiplier * player.Input.Movement.Value;
            //player.Movement = movementVelocity;
        }
        else
        {
            inputDirection =  speedMultiplier * lastInput;
            //player.Movement = movementVelocity;
        }
        */
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 targetDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized; //Dire��o

        if (targetDirection != Vector3.zero) //Dire��o Att
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        Vector3 targetVelocity = targetDirection; //Velocidade

        float currentAccel = inputDirection.magnitude > 0.1f ? acceleration : deceleration; //Acelera��o ou desacelera��o se magntude for maior que .1
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, currentAccel * Time.deltaTime);

        currentVelocity *= (1f - drag * Time.deltaTime); //Resist�ncia do chão

        //currentVelocity.y = Mathf.Lerp(currentVelocity.y, 0, surfaceFloatForce * Time.deltaTime); //Flutua��o (Ignoravel)
        currentVelocity.y = -5f;


        speedMultiplier = Mathf.MoveTowards(speedMultiplier, 1f, currentAccel * Time.deltaTime);
        speedMultiplier *= (1f - drag * Time.deltaTime);

        player.Animator.SetFloat("sprintVelocity", speedMultiplier);

        Debug.Log(currentVelocity * speedMultiplier);
        if (speedMultiplier <= 1f) 
        { 
            speedMultiplier = 1f; lastInput = Vector2.zero; inputDirection = Vector2.zero; 
            if(!isSprinting) {player.SwitchState<PlayerState_Grounded>();}
        }
        return currentVelocity * speedMultiplier; 
        //Vector2 movementVelocity = rotation * (inputDirection * movementSpeed);
        //directionalVelocity = movementVelocity;
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
        player.Move(CalculateVelocity(inputDirection, gravityDirection, Camera.main.transform.forward));
        /*
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
                speedMultiplier += Time.fixedDeltaTime * accelerationRate;
                directionalInput =  speedMultiplier * player.Input.Movement.Value;
                lastInput = directionalInput.normalized;
                //player.Movement = movementVelocity;
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
        */
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
        inputDirection = new Vector3(input.x, 0, input.y).normalized;
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
        float lastVelocity = Mathf.Abs(currentVelocity.x) > Mathf.Abs(currentVelocity.z) ? Mathf.Abs(currentVelocity.x) : Mathf.Abs(currentVelocity.z);
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
    */
    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {

        if (!isSprinting || !isInputing)
        {
            return HandleStop();
        }
        else
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            Vector3 targetDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized; //Dire��o

            if (targetDirection != Vector3.zero) //Dire��o Att
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            Vector3 targetVelocity = targetDirection * maxSpeed; //Velocidade

            float currentAccel = inputDirection.magnitude > 0.1f ? acceleration : deceleration; //Acelera��o ou desacelera��o se magntude for maior que .1
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, currentAccel * Time.deltaTime);

            currentVelocity *= (1f - drag * Time.deltaTime); //Resist�ncia do chão

            //currentVelocity.y = Mathf.Lerp(currentVelocity.y, 0, surfaceFloatForce * Time.deltaTime); //Flutua��o (Ignoravel)
            currentVelocity.y = -5f;


            speedMultiplier = Mathf.MoveTowards(speedMultiplier, maxSpeed, currentAccel * Time.deltaTime);
            speedMultiplier *= (1f - drag * Time.deltaTime);

            player.Animator.SetFloat("sprintVelocity", speedMultiplier);

            Debug.Log(currentVelocity * speedMultiplier);
            return currentVelocity * speedMultiplier;   
        }
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
