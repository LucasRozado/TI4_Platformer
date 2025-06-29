using System.Collections;
using UnityEngine;

public class ExplodeCactus : MonoBehaviour
{
    [SerializeField] private float explodeTimer = 2f; // Time before the cactus explodes
    [SerializeField] private float shootForce = 5f; // Force applied to the spikes when they are spawned
    [SerializeField] private bool triggered = false; // Flag to check if the cactus has exploded
    [SerializeField] private GameObject spikesPrefab; // Prefab of the spikes to be spawned]
    [SerializeField] private Animator animator; // Animator to control the cactus animation
    private Coroutine countdownCoroutine; // Reference to the countdown coroutine

    private void Start()
    {
        if (spikesPrefab == null)
        {
            Debug.LogWarning("Spikes prefab not found in the specified path.");
        }
        animator = GetComponentInChildren<Animator>(); // Get the animator component from the child objects of the cactus
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that entered the trigger is in the "Player" layer
        {
            triggered = true; // Set the flag to true to indicate that the cactus has exploded
            animator.SetBool("isExploding", true); // Set the animator parameter to trigger the explosion animation
            countdownCoroutine = StartCoroutine(CountdownCoroutine()); // Start the countdown coroutine
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that exited the trigger is in the "Player" layer
        {
            triggered = false; // Reset the triggered flag to allow reactivation
            animator.SetBool("isExploding", false); // Reset the animator parameter to stop the explosion animation
            if (countdownCoroutine != null) // Check if the countdown coroutine is running
            {
                StopCoroutine(countdownCoroutine); // Stop the countdown coroutine
                countdownCoroutine = null; // Reset the coroutine reference
            }
            explodeTimer = 2f; // Reset the explode timer to its initial value
        }
    }
    private IEnumerator CountdownCoroutine()
    {
        //float explodeTimer = this.explodeTimer; // Initialize the timer with the specified explode time
        while (explodeTimer > 0f)
        {
            if (triggered)
            {
                explodeTimer -= Time.deltaTime; // Decrease the timer by the time since the last frame
                if (explodeTimer <= 0f)
                {
                    explodeTimer = 0f; // Reset the timer to zero
                    triggered = false; // Reset the triggered flag to allow reactivation  
                    animator.SetTrigger("explode"); // Trigger the explosion animation
                    // Wait for the animation to finish before calling Explode
                    float waitTime = animator.GetCurrentAnimatorStateInfo(0).length; // Get the length of the current animation state
                    yield return new WaitForSeconds(waitTime - 0.65f); // Wait for the animation to finish
                    Explode(); // Call the explode method to handle the explosion effect
                }
            }
            yield return null; // Wait for the next frame
        }
    }
    public void Explode()
    {
        //GetComponent<MeshRenderer>().enabled = false; // Disable the mesh renderer to hide the cactus
        Collider[] colliders = GetComponents<Collider>(); // Get all colliders attached to the cactus
        foreach (Collider collider in colliders)
        {
            collider.enabled = false; // Disable all colliders to prevent further interactions
        }
        triggered = false; // Reset the triggered flag to allow reactivation
        SpawnSpikes(); // Call the method to spawn spikes
        explodeTimer = 2f; // Reset the explode timer to its initial value
    }
    private void SpawnSpikes()
    {
        float radius = 1f; // Radius of the octagon
        GameObject[] spike = new GameObject[8]; // Initialize the spike variable
        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.PI / 4; // Divide 360 degrees into 8 parts (45 degrees each)
            Vector3 position = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            spike[i] = Instantiate(spikesPrefab, position, Quaternion.identity);

            spike[i].transform.SetParent(transform); // Set the parent of the spike to the cactus
            Vector3 forceDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized; // Calculate the direction of the force based on the angle

            spike[i].GetComponent<Spike>().Shoot(forceDirection, shootForce); // Call the Shoot method on the spike to apply force in the direction of the angle
            Destroy(spike[i], 8f); // Destroy the spike after 5 seconds
        }
    }
}
