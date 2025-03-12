using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    static public Player instance;

    [SerializeField] private float movementSpeed = 5.0f; // metros por segundo
    [SerializeField] private float jumpStrength = 5.0f;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool isGrounded;

    private PlayerInput input;
    private CharacterController characterController;
    private void Start()
    {
        if (instance != null)
        { Debug.LogWarning("Existem duas instâncias de Player!"); }

        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        instance = this;
    }

    public float MovementSpeed => movementSpeed;
    public float JumpStrength => jumpStrength;
    public Vector3 Velocity => new(velocity.x, velocity.y, velocity.z);
    public bool IsGrounded => characterController.isGrounded;

    private void FixedUpdate()
    {
        HandleMovement(input, Physics.gravity);
    }

    private void HandleMovement(PlayerInput input, Vector3 gravity)
    {
        float perpendicularSpeed = Vector3.Dot(velocity, gravity.normalized);
        Vector3 perpendicularVelocity = perpendicularSpeed * gravity.normalized;

        if (input.Jump && characterController.isGrounded)
        {
            perpendicularVelocity = jumpStrength * -gravity.normalized;
        }
        else if (isGrounded)
        { perpendicularVelocity = Vector3.zero; }

        if (perpendicularSpeed < gravity.magnitude)
        {
            perpendicularVelocity += gravity * Time.deltaTime;

            if (perpendicularVelocity.magnitude > gravity.magnitude)
            { perpendicularVelocity = gravity; }
        }

        Vector3 cardinalVelocity = new Vector3(
            x: input.Movement.x,
            y: 0,
            z: input.Movement.y
        ) * movementSpeed;

        velocity = perpendicularVelocity + cardinalVelocity;

        CollisionFlags collision = characterController.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        const float forwardAngle = 30;

        if (hit.gameObject.CompareTag("CanClimb"))
        {
            if (Mathf.Abs(Vector3.Dot(transform.forward, hit.normal)) > Mathf.Cos(forwardAngle * Mathf.Deg2Rad))
            // Comparando o ângulo entre a frente do jogador e a normal da parede
            {
                transform.rotation = Quaternion.LookRotation(-hit.normal);
            }
        }
    }
}
