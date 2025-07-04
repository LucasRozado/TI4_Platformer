using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerState_Grounded : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;
    [SerializeField] private bool freezePosition = false;

    [SerializeField, Tooltip("In meters per second")]
    private float groundPull;

    [SerializeField] ParticleSystem dust;


    private PlayerState_Jump jump;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_Jump>();

        BindInputStart(player.Input.Jump, HandleJump);
        BindInputStart(player.Input.Sprint, HandleSprint);
        BindInputStart(player.Input.Interact, HandleInteraction);
        BindInputStart(player.Input.Spirit, SwitchReality);
    }

    protected override void EnterState()
    {
        player.PlayerAnimations.RunningAnimation(false);
        if (jump.IsBuffered)
        { HandleJump(); }
        dust.Play();
    }

    private void Update()
    {
        if (freezePosition)
        {
            player.Move(Vector3.zero);
            return;
        }
        else
        {            
            UpdateMovement();
            UpdateGroundPull();

            RotatePlayer();

            Vector3 velocity = CalculateVelocity();
            player.Move(velocity, HandleCollision);
        }
    }

    private void HandleSprint()
    {
        Debug.Log("Sprint");
        if (freezePosition == false)
        {
            player.SwitchState<PlayerState_GroundedRunning>();
        }
    }

    private void UpdateMovement()
    {

        Vector2 directionalInput = player.Input.Movement.Value;

        if (directionalInput != Vector2.zero)
        {
            player.RestoreStamina(Time.deltaTime * player.StaminaDepletionRate * 0.5f);  
        }
        else
        {
            player.RestoreStamina(Time.deltaTime * player.StaminaDepletionRate);
        }

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        Vector2 movementVelocity = rotation * (directionalInput * movementSpeed);
        directionalVelocity = movementVelocity;
        
        if (directionalVelocity != Vector2.zero)
        {
            player.PlayerAnimations.WalkAnimation(true);
        }
        else
        {
            player.PlayerAnimations.WalkAnimation(false);
        }
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
            jump.StartCoyoteTimer();
            return;
        }

        else if (hit != null && hit.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            player.SwitchState<PlayerState_Swim>();
            return;
        }
    }

    private void HandleInteraction()
    {
        if (freezePosition)
        {
            Debug.Log("Player is frozen, cannot interact.");
            return;
        }
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
        if (freezePosition == false)
        {
            player.SwitchState<PlayerState_Jump>();
        }
    }
    public void FreezePlayerPosition(bool freeze)
    {
        freezePosition = freeze;
    }

    protected override void ExitState()
    {
        dust.Stop();
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
