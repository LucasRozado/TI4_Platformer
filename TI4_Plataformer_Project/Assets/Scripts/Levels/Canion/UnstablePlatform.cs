using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UnstablePlatform : MonoBehaviour
{
    [SerializeField] private float destructionDelay = 2f; // Time before the platform is destroyed
    private Coroutine destructionCoroutine; // Reference to the destruction coroutine
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that entered the trigger is in the "Player" layer
        {
            destructionCoroutine = StartCoroutine(DestructionCoroutine()); // Start the destruction coroutine
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Check if the object that exited the trigger is in the "Player" layer
        {
            StopCoroutine(destructionCoroutine); // Stop the destruction coroutine
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
