using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.Windows;

public class PlayerState_GroundedRunning : PlayerState
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float turnSpeed = 5f;

    [SerializeField] private float movementSpeed = 5f;

    [Header("Values")]

    [SerializeField]private Vector3 currentVelocity;

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

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;

    [SerializeField, Tooltip("In meters per second")]
    private float groundPull;


    private PlayerState_Jump jump;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_Jump>();

        BindInputUpdate(player.Input.Movement, HandleMovement);
        BindInputStart(player.Input.Jump, HandleJump);
        BindInputStart(player.Input.Spirit, SwitchReality);
    }
    protected override void EnterState()
    {
        player.PlayerAnimations.RunningAnimation(true);
        player.PlayerAnimations.RunningSpeedAnimation(1f);
        if (jump.IsBuffered)
        { HandleJump(); }
        speedMultiplier = 1f;
    }
    protected override void ExitState()
    {
        player.PlayerAnimations.RunningSpeedAnimation(1f);
        isInputing = false;
        isSprinting = false;
        speedMultiplier = 1f;
    }
    
    private void Update()
    {
        player.PlayerAnimations.RunningSpeedAnimation(speedMultiplier);
        UpdateSprint();
        UpdateMovement();
        UpdateGroundPull();

        RotatePlayer();
    }
    private void UpdateGroundPull()
    {
        float groundPull =  movementSpeed / Mathf.Tan(-player.Slope);
        this.groundPull = groundPull;
    }
    private Vector3 HandleStop()
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

        Vector3 targetVelocity = targetDirection; //Velocidade

        float currentAccel = inputDirection.magnitude > 0.1f ? acceleration : deceleration; //Acelera��o ou desacelera��o se magntude for maior que .1
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, currentAccel * Time.deltaTime);

        currentVelocity *= (1f - drag * Time.deltaTime); //Resist�ncia do chão

        currentVelocity.y = -5f;

        speedMultiplier = Mathf.MoveTowards(speedMultiplier, 1f, currentAccel * Time.deltaTime);
        speedMultiplier *= (1f - drag * Time.deltaTime);

        if (speedMultiplier <= 1f) 
        { 
            speedMultiplier = 1f;
            inputDirection = Vector2.zero; 
            if(!isSprinting) {player.SwitchState<PlayerState_Grounded>();}
        }
        return currentVelocity * speedMultiplier; 
    }
    private void UpdateSprint()
    {
        if (player.Input.Sprint.Value >= 1f)
        { isSprinting = true; }
        else
        { isSprinting = false; }
    }
    private void UpdateMovement()
    {
        player.Move(
            CalculateVelocity(inputDirection, gravityDirection, Camera.main.transform.forward),
            HandleCollision
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
    private void HandleMovement(Vector2 input)
    {
    }
    private void HandleJump()
    {
        float lastVelocity = Mathf.Abs(currentVelocity.x) > Mathf.Abs(currentVelocity.z) ? Mathf.Abs(currentVelocity.x) : Mathf.Abs(currentVelocity.z);
        PlayerState_RunningJump runningJump = player.GetState<PlayerState_RunningJump>();
        runningJump.SetLastVelocity(lastVelocity);
        player.SwitchState(runningJump);
    }
    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        Vector2 input = player.Input.Movement.Value;
        if (input != Vector2.zero)
        { isInputing = true; }
        else
        { isInputing = false; }

        inputDirection = new Vector3(input.x, 0, input.y).normalized;

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

            currentVelocity.y = -5f;


            speedMultiplier = Mathf.MoveTowards(speedMultiplier, maxSpeed, currentAccel * Time.deltaTime);
            speedMultiplier *= (1f - drag * Time.deltaTime);

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
