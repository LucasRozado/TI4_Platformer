using System.Collections;
using UnityEngine;

public class FallingPilarTimer : MonoBehaviour
{
    [SerializeField] private float timer = 5f; 
    [SerializeField] private bool isActive = false; // Flag to check if the timer is active
    [SerializeField] private GameObject[] boardsBreaking; // Array of boards to be broken
    private Coroutine countdownCoroutine; // Reference to the countdown coroutine

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that entered the trigger is in the "Player" layer
        {
            isActive = true; // Set the timer to active
            countdownCoroutine = StartCoroutine(CountdownCoroutine()); // Start the countdown coroutine
        }
    }
    private IEnumerator CountdownCoroutine()
    {
        float fallTimer = timer; // Initialize the timer with the specified time
        while (fallTimer > 0f)
        {
            fallTimer -= Time.deltaTime; // Decrease the timer by the time since the last frame
            if (fallTimer <= 0f)
            {
                for (int i = 0; i < boardsBreaking.Length; i++)
                {
                    boardsBreaking[i].GetComponent<SpawnBoard>().Spawn(); // Call the break method on each board in the array
                }
                fallTimer = 0f; // Reset the timer to zero
            }
            yield return null; // Wait for the next frame
        }
    }
}
