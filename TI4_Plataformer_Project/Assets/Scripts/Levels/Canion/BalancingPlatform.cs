using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalancingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject weightPrefab; // Prefab to be spawned
    [SerializeField] private float spawnDelay = 1f; // Delay before the prefab is spawned
    [SerializeField] private float destroyDelay = 1f; // Delay before the prefab is destroyed
    [SerializeField] private float spawnHeight = 1f; // Height at which the prefab will be spawned
    [SerializeField] private bool isTriggered = false; // Flag to check if the trigger has been activated
    private Vector3 spawnPosition; // Position where the prefab will be spawned
    private Coroutine spawnCoroutine; // Reference to the spawn coroutine

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) // Check if the object that entered the trigger is in the "Player" layer
        {
            isTriggered = true; // Set the trigger flag to true
            spawnCoroutine = StartCoroutine(SpawnWeight(other.gameObject)); // Start the coroutine to spawn the weight
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7) // Check if the object that exited the trigger is in the "Player" layer
        {
            isTriggered = false; // Set the trigger flag to false
            StopCoroutine(spawnCoroutine); // Stop the spawn coroutine
            spawnCoroutine = null; // Reset the coroutine reference
        }
    }

    private IEnumerator SpawnWeight(GameObject player)
    {
        while(true)
        {
            if (isTriggered) // Check if the trigger has already been activated
            {
                spawnPosition = new Vector3(player.transform.position.x, player.transform.position.y + spawnHeight, player.transform.position.z); // Set the spawn position
                GameObject weight = Instantiate(weightPrefab, spawnPosition, Quaternion.identity); // Instantiate the weight prefab
                yield return new WaitForSeconds(spawnDelay); 
                Destroy(weight, destroyDelay); 
            }
        }
    }
}
