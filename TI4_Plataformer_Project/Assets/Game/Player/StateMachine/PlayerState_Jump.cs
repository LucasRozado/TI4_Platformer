using UnityEngine;
using System.Collections;

public class PlayerState_Jump : PlayerState
{

    [SerializeField] private float movementSpeedInMetersPerSecond = 5f;
    [SerializeField] private float maxJumpTime = 0.6f;
    [SerializeField] private float maxJumpHeight = 1.1f;
    private float initialJumpVelocity = 4f;
    private float jumpGravity = -13.75f;

    private readonly Vector3 gravityDirection = Physics.gravity.normalized;
    public override void Initialize()
    {
        BindInputUpdate(player.Input.Movement, HandleMovement);
        BindInputCancel(player.Input.Jump, CancelJump);
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

    protected override void EnterState()
    {
        MathJump();
        Debug.Log("Enter Jump");
        
        HandleCoroutine(HandleGravity_Coroutine());
    }
    public void MathJump()
    {
        float timeToApex = maxJumpTime / 2f;
        jumpGravity = (-2.0f * maxJumpHeight) / Mathf.Pow(timeToApex, 2.0f);
        initialJumpVelocity = (2.0f * maxJumpHeight) / timeToApex;

    }

    private IEnumerator HandleGravity_Coroutine()
    {
        player.Gravity = Vector3.up * initialJumpVelocity;
        float gravityStrength = jumpGravity;

        while (true)
        {
            float currentGravity = player.Gravity.y;
            if (currentGravity > 0)
            {
                float gravityAcceleration = gravityStrength * Time.deltaTime;
                currentGravity += gravityAcceleration;

                player.Gravity = currentGravity * Vector3.up;

                if (currentGravity <= 0)
                { player.SwitchState<PlayerState_Airbound>(); }
            }

            yield return null;
        }        
    }

    protected override void ExitState()
    {

    }

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        if (collision.flags == CollisionFlags.None) return;

        if (collision.flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Grounded>();
        }

        else if (collision.hit != null && collision.hit.gameObject.CompareTag("CanClimb"))
        {
            float angle = player.GetState<PlayerState_Climbing>().MaxHorizontalAngle_InDegrees;
            // Comparando o ângulo entre a frente do jogador e a normal da parede
            if (Mathf.Abs(Vector3.Dot(player.Forward, collision.hit.normal)) > Mathf.Cos(angle * Mathf.Deg2Rad))
            {
                player.Look(-collision.hit.normal);
                player.SwitchState<PlayerState_Climbing>();
            }
        }
    }

    private void HandleMovement(Vector2 input)
    {
        Vector2 movementVelocity = input * movementSpeedInMetersPerSecond;
        player.Movement = movementVelocity;
    }

    private void CancelJump()
    {
        player.SwitchState<PlayerState_Airbound>();
    }
}
