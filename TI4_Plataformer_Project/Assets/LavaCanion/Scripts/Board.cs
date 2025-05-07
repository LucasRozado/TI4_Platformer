using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f; // Force applied to the board when jumping
    [SerializeField] private Rigidbody boardRigidbody; // Reference to the Rigidbody component of the board

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boardRigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to this GameObject
        if (boardRigidbody == null)
        {
            Debug.LogWarning("Board Rigidbody not found on this GameObject.");
        }
        boardRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an upward force to the board
    }
}
