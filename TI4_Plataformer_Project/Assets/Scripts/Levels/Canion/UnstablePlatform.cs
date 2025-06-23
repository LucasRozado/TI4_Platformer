using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnstablePlatform : MonoBehaviour
{
    [SerializeField] private float destructionDelay = 2f; // Time before the platform is destroyed
    [SerializeField] private float destructionDelayAnimator = 2f; 
    [SerializeField] private bool destructionActive = false; // Flag to indicate if destruction is active
    [SerializeField] private Animator animator; // Animator to control the platform's animation
    private Coroutine destructionCoroutine; // Reference to the destruction coroutine

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(); // Get the Animator component if not assigned
        }
        destructionDelayAnimator = destructionDelay; // Initialize the animator's destruction delay
        animator.SetFloat("destructionDelay", destructionDelayAnimator); // Set the destruction delay in the animator
    }
    private void Update()
    {
        if (destructionActive)
        {
            destructionDelayAnimator -= Time.deltaTime; // Decrease the destruction delay over time
            animator.SetFloat("destructionDelay", destructionDelayAnimator); // Update the animator with the new delay
        }
        else
        {
            destructionDelayAnimator = destructionDelay; // Reset the animator's destruction delay if not active
            animator.SetFloat("destructionDelay", destructionDelayAnimator); // Update the animator with the reset delay
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that entered the trigger is in the "Player" layer
        {
            destructionCoroutine = StartCoroutine(DestructionCoroutine()); // Start the destruction coroutine
            destructionActive = true; // Set the destruction active flag
            animator.SetBool("destructionActive", true); // Set the animator to indicate destruction is active
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that exited the trigger is in the "Player" layer
        {
            StopCoroutine(destructionCoroutine); // Stop the destruction coroutine
            destructionActive = false; // Set the destruction active flag to false
            animator.SetBool("destructionActive", false); // Reset the animator to indicate destruction is no longer active
            destructionDelayAnimator = destructionDelay; // Reset the animator's destruction delay
            animator.SetFloat("destructionDelay", destructionDelayAnimator); // Update the animator with the reset delay
            destructionCoroutine = null; // Reset the coroutine reference
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = true; // Enable the collider to allow re-triggering
            }
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true; // Enable the MeshRenderer of the current GameObject
            }
        }
    }
    private IEnumerator DestructionCoroutine()
    {
        yield return new WaitForSeconds(destructionDelay); // Wait for the specified delay

        foreach (Collider col in GetComponents<Collider>())
        {
            col.enabled = false; // Disable the collider to prevent further triggers
        }
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // Disable the MeshRenderer of the current GameObject
        }
    }

}
