using System.Collections;
using UnityEngine;

public class SpawnBoard : MonoBehaviour
{
    [SerializeField] private float spawnTimer = 0.5f; // Time before the spawn occurs
    [SerializeField] private bool triggered = false; // Flag to check if the spawn has been triggered
    [SerializeField] private GameObject boardPrefab; // Prefab to be spawned
    private Coroutine countdownCoroutine; // Reference to the countdown coroutine

    private void Start()
    {
        if (boardPrefab == null)
        {
            Debug.LogWarning("Board prefab not found in the specified path.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // Check if the object that entered the trigger is in the "Player" layer
        {
            ActivateTimer(); // Call the method to start the countdown timer
            countdownCoroutine = StartCoroutine(CountdownCoroutine()); // Start the countdown coroutine
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // Check if the object that exited the trigger is in the "Player" layer
        {
            DeactivateTimer(); // Call the method to stop the countdown timer
            countdownCoroutine = null; // Stop the countdown coroutine
        }
    }
    void ActivateTimer()
    {
        if (!triggered)
        {
            triggered = true; // Set the flag to true to prevent multiple activations
        }
    }
    void DeactivateTimer()
    {
        triggered = false; // Reset the triggered flag to allow reactivation
    }
    private IEnumerator CountdownCoroutine()
    {
        float spawnTimer = this.spawnTimer; // Initialize the timer with the specified spawn time
        while (spawnTimer > 0f)
        {
            if (triggered)
            {
                spawnTimer -= Time.deltaTime; // Decrease the timer by the time since the last frame
                if (spawnTimer <= 0f)
                {
                    Spawn(); // Call the spawn method when the timer reaches zero
                    spawnTimer = 0f; // Reset the timer to zero
                    triggered = false; // Reset the triggered flag to allow reactivation  
                }
            }
            yield return null; // Wait for the next frame
        }
    }
    public void Spawn()
    {
        if (boardPrefab != null)
        {
            GetComponent<MeshRenderer>().enabled = false; // Disable the MeshRenderer of the current GameObject
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false; // Disable all colliders attached to the current GameObject
            }
            GameObject spawnedBoard = null; // Initialize the spawned board variable
            for (int i = 0; i < Random.Range(1, 6); i++)
            {
                spawnedBoard = Instantiate(boardPrefab, transform.position, Quaternion.Euler(0, Random.Range(0f, 360f), 0)); 
                // Spawn the board prefab at the current position with a random Y-axis rotation
            }
            spawnedBoard.SetActive(true); // Activate the spawned board
        }
        else
        {
            Debug.LogWarning("Board prefab is not assigned."); // Log a warning if the prefab is not assigned
        }
    }
}
