using UnityEngine;

public class PlayerState_Climbing : PlayerState
{
    [Header("Values")]

    [SerializeField, Tooltip("In meters per second")]
    private float movementSpeed = 5f;

    public float MaxHorizontalAngle => maxHorizontalAngle;
    [SerializeField, Range(0, 90), Tooltip("In degrees")]
    private float maxHorizontalAngle = 30f;

    [Header("Observables")]

    [SerializeField, Tooltip("In meters per second")]
    private Vector2 directionalVelocity;

    [Tooltip("In meters")]
    private float gripReach;

    [SerializeField, Tooltip("In degrees")]
    private float wallDirection;


    private PlayerState_Jump jump;
    public override void Initialize()
    {
        jump = player.GetState<PlayerState_Jump>();

        gripReach = Mathf.Sin(maxHorizontalAngle * Mathf.Deg2Rad);

        BindInputStart(player.Input.Jump, HandleJump);
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
        player.PlayerAnimations.ClimbTypeAnimation(0);
    }

    private void HandleJump()
    {
        player.SwitchState<PlayerState_Jump>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateGrip();

        RotatePlayer();

        Vector3 velocity = CalculateVelocity();
        player.Move(velocity);

        CheckForFall();
    }

    private void UpdateMovement()
    {
        Vector2 directionalInput = player.Input.Movement.Value;

        Vector2 movementVelocity = directionalInput * movementSpeed;
        directionalVelocity = movementVelocity;

        if (directionalVelocity != Vector2.zero)
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
            player.PlayerAnimations.ClimbTypeAnimation(0);
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
            jump.StartCoyoteTimer();
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
