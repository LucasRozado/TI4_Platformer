using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "PlayerMovementClimb", menuName = "Scriptable Objects/PlayerMovement/Climb")]
public class PlayerMovementClimb : PlayerMovement
{
    [SerializeField, Range(0, 90)] private float horizontalAngle = 30;
    [SerializeField] private float handsDistance = 0.4f;
    [SerializeField] private float handsHeight = 1.5f;
    public float HandsReach => Mathf.Sin(horizontalAngle * Mathf.Deg2Rad);

    public override Vector3 Velocity(Transform camera, Transform player, Vector3 currentVelocity, Vector2 movementInput, float movementSpeed)
    {
        Vector3 relativeVelocity = new Vector3(
            x: movementInput.x,
            y: movementInput.y,
            z: 0
        ) * movementSpeed;

        Vector3 velocity = player.rotation * relativeVelocity;
        return velocity;
    }

    public override Vector3 Gravity(Transform player, Vector3 currentGravity)
    {
        Vector3 handsCenterPosition = player.position + player.up * handsHeight;
        Vector3 handLeftPosition = handsCenterPosition - player.right * handsDistance;
        Vector3 handRightPosition = handsCenterPosition + player.right * handsDistance;

        Ray handLeftGrip = new(handLeftPosition, player.forward);
        Ray handRightGrip = new(handRightPosition, player.forward);

        bool leftHandHit = Physics.Raycast(handLeftGrip, out RaycastHit leftHandInfo, HandsReach);
        bool rightHandHit = Physics.Raycast(handLeftGrip, out RaycastHit rightHandInfo, HandsReach);

        Vector3 gravity = currentGravity;

        if (leftHandHit && rightHandHit)
        {
            float angle = Mathf.Atan2((rightHandInfo.distance - leftHandInfo.distance), (handsDistance * 2)) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, angle / 2, 0f);
            gravity = rotation * player.forward;
        }

        return gravity;
    }

    public override Quaternion Rotation(Transform player, Vector3 currentVelocity, Vector3 currentGravity)
    {
        return Quaternion.LookRotation(currentGravity);
    }

    private Vector3 ClimbJump(Transform player, float movementSpeed, float jumpStrength)
    {
        Vector3 horizontalVelocity = -player.forward * movementSpeed;
        Vector3 verticalVelocity = jumpStrength * Vector3.up;
        return verticalVelocity + horizontalVelocity;
    }
}
