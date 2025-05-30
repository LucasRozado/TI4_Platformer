using UnityEngine;

public class PlayerState_Swim : PlayerState
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSwimSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float turnSpeed = 5f;

    [Header("Water Physics")]
    [SerializeField] private float waterDrag = 0.5f;
    [SerializeField] private float surfaceFloatForce = 2f;

    [Header("Jump Settings")]
    [SerializeField] private bool enableWaterJump = true;
    [SerializeField] private float swimJumpForce = 2f;

    private Vector3 currentVelocity;
    private Vector3 swimInputDirection;

    public override void Initialize()
    {
        BindInputUpdate(player.Input.Movement, HandleMovementInput);

        if (enableWaterJump)
        {
            BindInputStart(player.Input.Jump, HandleSwimBoost);
        }
        BindInputStart(player.Input.Spirit, SwitchReality);
    }

    protected override void EnterState()
    {
        player.PlayerAnimations.SwimAnimation(true);
        currentVelocity = player.Velocity * 0.7f;
        currentVelocity.y = 0;
    }

    protected override void ExitState()
    {
        player.PlayerAnimations.SwimAnimation(false);
        player.PlayerAnimations.SwimSpeedAnimation(0f);
    }

    private void HandleMovementInput(Vector2 input)
    {
        swimInputDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    private void HandleSwimBoost()//Pulo na [agua, sem gravidade ainda
    {
        if (enableWaterJump)
        {
            currentVelocity.y = swimJumpForce;
        }
    }

    protected override Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 targetDirection = (cameraForward * swimInputDirection.z + cameraRight * swimInputDirection.x).normalized; //Dire��o

        if (targetDirection != Vector3.zero) //Dire��o Att
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        Vector3 targetVelocity = targetDirection * maxSwimSpeed; //Velocidade

        float currentAccel = swimInputDirection.magnitude > 0.1f ? acceleration : deceleration; //Acelera��o ou desacelera��o se magntude for maior que .1
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, currentAccel * Time.deltaTime);

        currentVelocity *= (1f - waterDrag * Time.deltaTime); //Resist�ncia da agua

        //currentVelocity.y = Mathf.Lerp(currentVelocity.y, 0, surfaceFloatForce * Time.deltaTime); //Flutua��o (Ignoravel)
        currentVelocity.y = -5f;
        if (Mathf.Abs(player.Input.Movement.Value.y) > Mathf.Abs(player.Input.Movement.Value.x))
        {
            player.PlayerAnimations.SwimSpeedAnimation(Mathf.Abs(currentVelocity.z));
        }
        else if (Mathf.Abs(player.Input.Movement.Value.x) > Mathf.Abs(player.Input.Movement.Value.y))
        {
            player.PlayerAnimations.SwimSpeedAnimation(Mathf.Abs(currentVelocity.x));
        }
        else
        {
            player.PlayerAnimations.SwimSpeedAnimation(Mathf.Abs(currentVelocity.x));
        }

        
        return currentVelocity;
    }

    protected override void HandleCollisionUpdate(Player.ControllerCollision collision)
    {
        if (!collision.flags.HasFlag(CollisionFlags.Below))
        {
            player.SwitchState<PlayerState_Airbound>();
            return;
        }

        else if (collision.hit.gameObject.layer != LayerMask.NameToLayer("Water"))
        {
            player.SwitchState<PlayerState_Grounded>();
            return;
        }
    }

    public void SetWaterJumpEnabled(bool enabled)
    {
        enableWaterJump = enabled;

        if (enabled)
        {
            BindInputStart(player.Input.Jump, HandleSwimBoost);
        }
        else
        {
            BindInputCancel(player.Input.Jump, HandleSwimBoost);
        }
    }
}