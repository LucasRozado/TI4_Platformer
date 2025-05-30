using UnityEngine;

public class PlayerState_Dead : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In seconds")]
    private float duration = 2f;

    [Header("Observables")]

    [Tooltip("In seconds")]
    public float remainingDuration;

    [Tooltip("In meters per second per second")]
    private float gravityAcceleration;

    [Tooltip("In meters per second")]
    public float verticalVelocity;

    private PlayerState_Jump jump;
    private PlayerState_Airbound airbound;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_Jump>();
        airbound = player.GetState<PlayerState_Airbound>();
    }

    protected override void EnterState()
    {
        player.ToggleCollider(false);
        player.PlayerAnimations.RunningAnimation(false);

        CalculateParameters();
        remainingDuration = duration;
    }

    private void CalculateParameters()
    {
        gravityAcceleration = (2f * jump.DefaultHeight) / -Mathf.Pow(jump.DefaultFallTime, 2);
    }

    private void Update()
    {
        UpdateGravity();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity);

        if (remainingDuration > 0)
        {
            if (remainingDuration > Time.deltaTime)
            { remainingDuration -= Time.deltaTime; }
            else
            { Respawn(); }
        }
    }

    private void UpdateGravity()
    {
        if (verticalVelocity > -airbound.TerminalVelocity)
        {
            float velocityFromGravity = gravityAcceleration * Time.deltaTime;
            verticalVelocity += velocityFromGravity;

            if (verticalVelocity < -airbound.TerminalVelocity)
            { verticalVelocity = -airbound.TerminalVelocity; }
        }
    }
    private Vector3 CalculateVelocity()
    {
        Vector3 velocity = new()
        {
            x = 0,
            y = verticalVelocity,
            z = 0,
        };

        return velocity;
    }

    private void Respawn()
    {
        GameManager.Instance.ResetToCheckPoint();
        player.SwitchState<PlayerState_Airbound>();
    }

    protected override void ExitState()
    {
        remainingDuration = 0;
        player.ToggleCollider(true);
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
