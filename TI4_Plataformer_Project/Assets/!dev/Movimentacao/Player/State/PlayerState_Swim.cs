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
    }

    protected override void EnterState()
    {
        currentVelocity = player.Velocity * 0.7f;
        currentVelocity.y = 0;
    }

    protected override void ExitState()
    {
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

        Vector3 targetDirection = (cameraForward * swimInputDirection.z + cameraRight * swimInputDirection.x).normalized; //Direção

        if (targetDirection != Vector3.zero) //Direção Att
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        Vector3 targetVelocity = targetDirection * maxSwimSpeed; //Velocidade

        float currentAccel = swimInputDirection.magnitude > 0.1f ? acceleration : deceleration; //Aceleração ou desaceleração se magntude for maior que .1
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, currentAccel * Time.deltaTime);

        currentVelocity *= (1f - waterDrag * Time.deltaTime); //Resistência da agua

        //currentVelocity.y = Mathf.Lerp(currentVelocity.y, 0, surfaceFloatForce * Time.deltaTime); //Flutuação (Ignoravel)
        currentVelocity.y = -5f;

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