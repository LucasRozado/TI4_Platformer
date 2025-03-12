using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementWalk", menuName = "Scriptable Objects/PlayerMovement/Walk")]
public class PlayerMovementWalk : PlayerMovement
{
    private Vector3 forward;
    public override Vector3 Velocity(Transform camera, Transform player, Vector3 currentVelocity, Vector2 movementInput, float movementSpeed)
    {
        Vector3 relativeVelocity = new Vector3(
            x: movementInput.x,
            y: 0,
            z: movementInput.y
        ) * movementSpeed;

        if (movementInput == Vector2.zero)
        { forward = Vector3.Scale(camera.forward, new Vector3(1f, 0f, 1f)); }
        
        Vector3 velocity = Quaternion.LookRotation(forward) * relativeVelocity;
        return velocity;
    }

    public override Vector3 Gravity(Transform player, Vector3 currentGravity)
    {
        Vector3 velocity = Physics.gravity * Time.fixedDeltaTime;
        return velocity;
    }

    public override Quaternion Rotation(Transform player, Vector3 currentVelocity, Vector3 currentGravity)
    {
        return Quaternion.LookRotation(currentVelocity);
    }
}
