using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementJump", menuName = "Scriptable Objects/PlayerMovement/Jump")]
public class PlayerMovementJump : PlayerMovement
{
    public override Vector3 Gravity(Transform player, Vector3 currentGravity)
    {
        throw new System.NotImplementedException();
    }

    public override Quaternion Rotation(Transform player, Vector3 currentVelocity, Vector3 currentGravity)
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 Velocity(Transform camera, Transform player, Vector3 currentVelocity, Vector2 movementInput, float movementSpeed)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 WalkVelocity(Vector2 movementInput, float movementSpeed, Vector3 gravity, Vector3 currentVelocity)
    {
        Vector3 cardinalVelocity = new Vector3(
            x: movementInput.x,
            y: 0,
            z: movementInput.y
        ) * movementSpeed;

        Vector3 verticalVelocity = gravity * Time.fixedDeltaTime;

        // Velocidade terminal
        if (Vector3.Dot(currentVelocity, gravity) > gravity.magnitude)
        { verticalVelocity = gravity; }

        return cardinalVelocity + verticalVelocity;
    }
}
