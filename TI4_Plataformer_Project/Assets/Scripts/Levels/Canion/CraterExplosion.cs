using UnityEngine;

public class CraterExplosion : MonoBehaviour
{
    [SerializeField] private int groundAmmount = 1; // Number of ground prefabs to be destroyed
    [SerializeField] private float explosionTimer = 0.5f; // Time before the explosion occurs
    [SerializeField] private bool triggered = false; // Flag to check if the explosion has been triggered
    [SerializeField] private GameObject[] groundPrefab; // Array to hold the ground prefabs
    [SerializeField] private GameObject lavaPrefab;

    void Start()
    {
        groundPrefab = new GameObject[groundAmmount]; // Initialize the array with the specified size
        for (int i = 0; i < groundAmmount; i++)
        {
            groundPrefab[i] = transform.Find("GroundPrefab" + i)?.gameObject; // Find the ground prefabs by name
            if (groundPrefab[i] == null)
            {
                Debug.LogWarning($"GroundPrefab{i} not found as a child of this GameObject.");
            }
        }
        lavaPrefab = transform.Find("LavaPrefab")?.gameObject;
        if (lavaPrefab == null)
        {
            Debug.LogWarning("lavaPrefab not found as a child of this GameObject.");
        }
        lavaPrefab.SetActive(false); // Ensure the lava prefab is inactive at the start
    }
    private void Update()
    {
        Countdown(); // Call the countdown method every frame
    }
    public void ActivateTimer()
    {
        if (!triggered)
        {
            triggered = true; // Set the flag to true to prevent multiple activations
        }
    }
    public void Countdown()
    {
        if (triggered)
        {
            explosionTimer -= Time.deltaTime; // Decrease the timer by the time since the last frame
            if (explosionTimer <= 0f)
            {
                Explode(); // Call the explode method when the timer reaches zero
                explosionTimer = 0f; // Reset the timer to zero
                triggered = false; // Reset the triggered flag to allow reactivation  
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) // Check if the object that entered the trigger is in the "Player" layer
        {
            ActivateTimer(); // Call the method to start the countdown timer
        }
    }
    private void Explode()
    {
        for (int i = 0; i < groundAmmount; i++)
        {
            if (groundPrefab[i] != null)
            {
                Destroy(groundPrefab[i]); // Destroy the ground prefab to create the explosion effect
            }
            else
            {
                Debug.LogWarning($"GroundPrefab{i} is null, cannot deactivate.");
            }
        }
        lavaPrefab.SetActive(true); // Activate the lava prefab to create the explosion effect
        lavaPrefab.GetComponent<Lava>().ActivateLava(); // Call the ActivateLava method on the lava prefab to start the lava effect
        Destroy(lavaPrefab, 5f); // Destroy the lava prefab after 5 seconds
        Destroy(this); // Destroy this script component to prevent further countdowns and explosions
    }
}
