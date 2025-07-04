using UnityEngine;

public class PlayerState_Climbing : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 1f;

    public float MaxHorizontalAngle => maxHorizontalAngle;
    [SerializeField, Range(0, 90), Tooltip("In degrees")]
    private float maxHorizontalAngle = 30f;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [SerializeField] bool isJumping = false;

    [Tooltip("In meters")]
    private float gripReach;

    [SerializeField, Tooltip("In degrees")]
    private float wallDirection;


    private PlayerState_ClimbingJump jump;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_ClimbingJump>();

        gripReach = Mathf.Sin(maxHorizontalAngle * Mathf.Deg2Rad);

        BindInputStart(player.Input.Jump, ResetJumpForce);
        BindInputUpdate(player.Input.Jump, IncreaseJumpForce);
        BindInputCancel(player.Input.Jump, HandleJump);

        BindInputStart(player.Input.Spirit, SwitchReality);
    }

    protected override void EnterState()
    {
        player.PlayerAnimations.ClimbAnimation(true);
        player.PlayerAnimations.ClimbTypeAnimation(0);
        wallDirection = player.transform.rotation.eulerAngles.y;
    }

    protected override void ExitState()
    {
        player.PlayerAnimations.ClimbAnimation(false);
        isJumping = false;
        player.PlayerAnimations.ClimbTypeAnimation(0);
    }

    private void ResetJumpForce()
    {
        jumpForce = 1f; // Reset jump force to initial value
    }

    private void IncreaseJumpForce(float input)
    {
        if (input >= 1f)
        {
            isJumping = true;
            player.PlayerAnimations.JumpAnimation(3);
        }
        else
        {
            isJumping = false;
        }
        //player.SwitchState<PlayerState_Jump>();
    }
    private void HandleJump()
    {
        jump.IncreaseDefaultHeight(jumpForce);
        player.SwitchState(jump);
    }

    private void Update()
    {
        UpdateMovement();
        UpdateGrip();

        RotatePlayer();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity);

        if (isJumping)
        {
            jumpForce += Time.deltaTime; // Increase jump force over time
            jumpForce = Mathf.Clamp(jumpForce, 1f, 5f); // Clamp to a maximum value
        }
        else
        {
            jumpForce = 1f; // Reset jump force if not jumping
        }

        CheckForFall();
    }

    private void UpdateMovement()
    {
        Vector2 directionalInput = player.Input.Movement.Value;
        if (isJumping == false)
        {
            if (player.Stamina <= 0)
            {
                isJumping = false; // Stop jumping when out of stamina
                player.PlayerAnimations.JumpAnimation(0); // Reset jump animation
                if (player.Input.Movement.Value.y > 0)
                {
                    directionalVelocity = new Vector2(directionalInput.x, -directionalInput.y) * movementSpeed; // Prevent upward movement when out of stamina
                }
                else if (player.Input.Movement.Value.y < 0)
                {
                    directionalVelocity = new Vector2(directionalInput.x * movementSpeed, directionalInput.y * movementSpeed * 2); // Prevent horizontal movement when out of stamina
                }
                else
                {
                    directionalVelocity = new Vector2(directionalInput.x, -5); // Stop all movement when out of stamina
                }
            }
            else
            {
                Vector2 movementVelocity = directionalInput * movementSpeed;
                directionalVelocity = movementVelocity;
            }

            if (directionalVelocity != Vector2.zero)
            {
                player.DepleteStamina(Time.deltaTime);
                if (player.Stamina > player.MaxStamina * 0.25f)
                {
                    if (player.Input.Movement.Value.y > 0)
                    {
                        player.PlayerAnimations.ClimbTypeAnimation(1);
                    }
                    else if (player.Input.Movement.Value.x < 0)
                    {
                        player.PlayerAnimations.ClimbTypeAnimation(2);
                    }
                    else if (player.Input.Movement.Value.y < 0)
                    {
                        player.PlayerAnimations.ClimbTypeAnimation(3);
                    }
                    else if (player.Input.Movement.Value.x > 0)
                    {
                        player.PlayerAnimations.ClimbTypeAnimation(4);
                    }
                    else
                    {
                        player.PlayerAnimations.ClimbTypeAnimation(0);
                    }
                }
                else
                {
                    player.PlayerAnimations.ClimbTypeAnimation(-1); // Reset animation when out of stamina
                }
            }
            else
            {
                player.DepleteStamina(Time.deltaTime * 0.5f); // Deplete stamina slower when not moving
                if (player.Stamina > 0)
                {
                    player.PlayerAnimations.ClimbTypeAnimation(0);
                }
                else
                {
                    player.PlayerAnimations.ClimbTypeAnimation(-1); // Reset animation when out of stamina
                }
            }
        }
        else
        {
            player.DepleteStamina(Time.deltaTime * 0.25f); // Deplete stamina slower when jumping
            if (player.Stamina <= 0)
            {
                isJumping = false; // Stop jumping when out of stamina
                player.PlayerAnimations.JumpAnimation(0); // Reset jump animation
            }
            directionalVelocity = Vector2.zero; // Stop movement when jumping
        }
    }

    private void CalculateGrip(out Ray leftGrip, out Ray rightGrip, out float distanceBetweenGrips)
    {
        Transform leftInteractionChecker = player.LeftInteractionChecker;
        Transform rightInteractionChecker = player.RightInteractionChecker;

        distanceBetweenGrips = Vector3.Distance(leftInteractionChecker.position, rightInteractionChecker.position);

        leftGrip = new(leftInteractionChecker.position, leftInteractionChecker.forward);
        rightGrip = new(rightInteractionChecker.position, rightInteractionChecker.forward);
    }

    private void UpdateGrip()
    {
        CalculateGrip(out Ray leftGrip, out Ray rightGrip, out float distanceBetweenGrips);

        bool leftGripHits = Physics.Raycast(leftGrip, out RaycastHit leftGripHit, gripReach);
        bool rightGripHits = Physics.Raycast(rightGrip, out RaycastHit rightGripHit, gripReach);

        if (leftGripHits && rightGripHits)
        {
            float angle = Mathf.Atan2((leftGripHit.distance - rightGripHit.distance), (distanceBetweenGrips)) * Mathf.Rad2Deg;

            if (Vector3.Dot(leftGripHit.normal, rightGripHit.normal) >= Mathf.Cos(Mathf.Deg2Rad * maxHorizontalAngle))
            { wallDirection += angle; }
            else if (Mathf.Sign(directionalVelocity.x) == Mathf.Sign(angle))
            { directionalVelocity.x = 0; }
        }
    }

    private void CheckForFall()
    {
        CalculateGrip(out Ray leftGrip, out Ray rightGrip, out _);

        bool leftGripHits = Physics.Raycast(leftGrip, gripReach);
        bool rightGripHits = Physics.Raycast(rightGrip, gripReach);

        if (!leftGripHits && !rightGripHits)
        {
            player.SwitchState<PlayerState_Airbound>();
        }
    }

    private void RotatePlayer()
    {
        Quaternion rotation = Quaternion.Euler(0, wallDirection, 0);
        player.Look(rotation);
    }

    private Vector3 CalculateVelocity()
    {
        Vector3 velocity = player.transform.rotation * new Vector3()
        {
            x = directionalVelocity.x,
            y = directionalVelocity.y,
            z = 0,
        };
        return velocity;
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
