using System.Collections;
using UnityEngine;

public class PlayerState_RunningJump : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;
    private float lastVelocity;

    public float DefaultHeight => defaultHeight;
    [SerializeField, Tooltip("In meters")]
    private float defaultHeight = 1.1f;

    [SerializeField, Tooltip("In seconds")]
    private float defaultTimeToApex = 0.4f;

    public float DefaultFallTime => defaultFallTime;
    [SerializeField, Tooltip("In seconds")]
    private float defaultFallTime = 0.3f;

    public float BufferTime => bufferTime;
    [SerializeField, Tooltip("In seconds")]
    private float bufferTime = 0.1f;

    [SerializeField, Tooltip("In seconds")]
    private float coyoteTime = 0.5f;

    [Header("Observables")]

    [Tooltip("In meters per second")]
    private float initialJumpVelocity;

    [Tooltip("In meters per second per second")]
    private float gravityAcceleration;

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [SerializeField, Tooltip("In meters per second")]
    private float verticalVelocity;

    [SerializeField, Tooltip("In seconds")]
    private float coyoteTimer;

    [SerializeField]
    private bool isSprinting = false;
    private PlayerState_Airbound airbound;
    public override void Initialize()
    {
        airbound = player.GetState<PlayerState_Airbound>();

        BindInputCancel(player.Input.Jump, HandleJumpCancel);
    }

    protected override void EnterState()
    {
        player.Animator.SetTrigger("jump");

        CalculateParameters();

        verticalVelocity = initialJumpVelocity;
        coyoteTimer = 0;

        if (IsCancelBuffered)
        { HandleJumpCancel(); }
    }

    private void CalculateParameters()
    {
        initialJumpVelocity = (2f * defaultHeight) / defaultTimeToApex;
        gravityAcceleration = (2f * defaultHeight) / -Mathf.Pow(defaultTimeToApex, 2);
    }

    private void HandleJumpCancel()
    {
        player.SwitchState(airbound);
        airbound.verticalVelocity = verticalVelocity;
    }

    public bool IsBuffered
    {
        get
        {
            if (player.Input.Jump.LastStart == Time.time) return false;

            float timeSinceLastBuffer = Time.time - player.Input.Jump.LastStart;
            return timeSinceLastBuffer < bufferTime;
        }
    }

    public bool IsCancelBuffered
    {
        get
        {
            if (!IsBuffered) return false;
            if (player.Input.Jump.Value != 0) return false;

            float timeSinceLastBuffer = Time.time - player.Input.Jump.LastUpdate;
            return timeSinceLastBuffer < bufferTime;
        }
    }

    public bool IsOnCoyoteTime => coyoteTimer > 0;

    private Coroutine coyoteTimerCoroutine;
    public void StartCoyoteTimer()
    {
        if (coyoteTimerCoroutine != null)
        { StopCoroutine(coyoteTimerCoroutine); }

        coyoteTimer = coyoteTime;
        coyoteTimerCoroutine = StartCoroutine(CoyoteTime_Coroutine());
    }
    private IEnumerator CoyoteTime_Coroutine()
    {
        while (coyoteTimer > 0)
        {
            yield return null;
            coyoteTimer -= Time.deltaTime;
        }
        coyoteTimer = 0;
        coyoteTimerCoroutine = null;
    }
    public float SetLastVelocity(float velocity)
    {
        lastVelocity = velocity;
        return lastVelocity;
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

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0;

        Quaternion rotation;
        if (cameraDirection != Vector3.zero)
        { rotation = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y); }
        else
        { rotation = Quaternion.identity; }

        Vector2 movementVelocity = rotation * (directionalInput * movementSpeed);
        directionalVelocity = movementVelocity * lastVelocity;
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
