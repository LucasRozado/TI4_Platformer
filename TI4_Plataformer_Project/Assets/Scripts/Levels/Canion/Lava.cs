using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private Rigidbody lavaRigidbody; // Reference to the Rigidbody component of the lava object
    void Start()
    {
        lavaRigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to this GameObject
        lavaRigidbody.useGravity = false; // Disable gravity for the lava object at the start
        if (lavaRigidbody == null)
        {
            Debug.LogWarning("Lava Rigidbody not found on this GameObject.");
        }
    }
    public void ActivateLava()
    {
        if (lavaRigidbody != null)
        {
            lavaRigidbody.isKinematic = false; // Set the Rigidbody to be non-kinematic to allow physics interactions
            lavaRigidbody.AddForce(Vector3.up * 20f, ForceMode.Impulse); // Apply an upward force to the lava object
            lavaRigidbody.useGravity = true; // Enable gravity for the lava object
        }
    }
}
