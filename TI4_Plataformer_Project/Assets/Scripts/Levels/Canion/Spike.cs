using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float spikeForce = 5f;
    [SerializeField] private Rigidbody spikeRigidbody; // Reference to the Rigidbody component of the spike

    private void Awake()
    {
        spikeRigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to this GameObject
        if (spikeRigidbody == null)
        {
            Debug.LogWarning("Spike Rigidbody not found on this GameObject.");
        }
    }

    public void Shoot(Vector3 direction)
    {
        Debug.Log("Spike shot in direction: " + direction); 
        spikeRigidbody.AddForce(direction * spikeForce, ForceMode.Impulse);
    }
    public void Shoot(Vector3 direction, float shootForce)
    {
        Debug.Log("Spike shot in direction: " + direction + ", shootForce equals: " + shootForce); 
        spikeRigidbody.AddForce(direction * shootForce, ForceMode.Impulse); 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // Check if the spike collides with the player
        {
            Debug.Log("Spike hit the player!"); 
            Player.instance.TakeDamage();
            transform.SetParent(collision.transform); // Set the spike as a child of the player
            // Add any additional logic for when the spike hits the player here
        }
        spikeRigidbody.isKinematic = true; // Set the Rigidbody to kinematic to stop it from moving
    }
}
