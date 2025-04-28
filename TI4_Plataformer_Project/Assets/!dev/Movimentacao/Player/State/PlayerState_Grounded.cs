using System.Collections;
using UnityEngine;

public class PlayerState_Grounded : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [SerializeField, Tooltip("In meters per second")]
    private float groundPull;


    private PlayerState_Jump jump;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_Jump>();

        BindInputStart(player.Input.Jump, HandleJump);
        BindInputStart(player.Input.Sprint, HandleSprint);
        BindInputStart(player.Input.Interact, HandleInteraction);
    }

    protected override void EnterState()
    {
        if (jump.IsBuffered)
        { HandleJump(); }
    }

    private void Update()
    {
        UpdateMovement();
        UpdateGroundPull();

        RotatePlayer();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity,
            onFlagsUpdate: HandleCollisionUpdate
        );
    }

    private void HandleSprint()
    {
        Debug.Log("Sprint");
        player.Animator.SetBool("isWalking", false);
        player.Animator.SetBool("isIdleGround", false);
        player.Animator.SetBool("isSprintingIdle", true);
        player.Animator.SetTrigger("fall");
        
        player.SwitchState<PlayerState_GroundedRunning>();
    }
    
    private void UpdateMovement()
    {
        Vector2 directionalInput = player.Input.Movement.Value;

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        if (directionalInput != Vector2.zero)
        {
            player.Animator.SetBool("isWalking", true);
            player.Animator.SetBool("isIdleGround", false);
        }
        else
        {
            player.Animator.SetBool("isWalking", false);
            player.Animator.SetBool("isIdleGround", true);
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

    protected void HandleCollisionUpdate(CollisionFlags flags, ControllerColliderHit hit)
    {
        if (!flags.HasFlag(CollisionFlags.Below))
        {
            player.Move(new(0, -groundPull, 0));
            player.SwitchState<PlayerState_Airbound>();
            jump.StartCoyoteTimer();
        }
    }

    private void HandleInteraction()
    {
        Debug.Log("Interaction");
        Transform checkL = player.GetInteractChecks(0);
        Transform checkR = player.GetInteractChecks(1);
        RaycastHit hitL;
        RaycastHit hitR;

        Physics.Raycast(checkL.position, checkL.forward, out hitL, player.InteractDistance, player.CanInteract);
        Physics.Raycast(checkR.position, checkR.forward, out hitR, player.InteractDistance, player.CanInteract);

        Debug.Log(hitL.collider);
        Debug.Log(hitR.collider);
        if (hitL.collider != null && hitL.collider == hitR.collider)
        {
            Debug.Log("Target acquired");
            if (hitL.collider.TryGetComponent(out Interactable interactable))
            {
                player.Look(-hitL.normal);
                interactable.InteractWith(player);
                Debug.Log("Interact Done");
            }
        }
    }

    private void HandleJump()
    {
        player.Animator.SetBool("isWalking", false);
        player.Animator.SetBool("isIdleGround", false);
        player.Animator.SetBool("isSprinting", false);
        player.Animator.SetBool("isSprintingIdle", false);

        player.Animator.SetTrigger("jump");
        player.SwitchState<PlayerState_Jump>();
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
