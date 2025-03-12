using UnityEngine;

public abstract class PlayerMovement : ScriptableObject
{
    public abstract Vector3 Velocity(Transform camera, Transform player, Vector3 currentVelocity, Vector2 movementInput, float movementSpeed);
    public abstract Vector3 Gravity(Transform player, Vector3 currentGravity);
    public abstract Quaternion Rotation(Transform player, Vector3 currentVelocity, Vector3 currentGravity);
}
