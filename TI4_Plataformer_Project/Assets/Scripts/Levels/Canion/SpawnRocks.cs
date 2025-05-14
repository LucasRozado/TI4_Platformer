using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnRocks : MonoBehaviour
{
    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 spawnRotation;
    [SerializeField] private float spawnInterval = 2f;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        if (rockPrefabs.Length == 0)
        {
            Debug.LogError("No rock prefabs assigned to SpawnRocks script on " + gameObject.name);
            return;
        }
        spawnPosition = transform.position;
        // Start the coroutine to spawn rocks
        StartCoroutine();
    }
    public void StartCoroutine()
    {
        // Start the coroutine to spawn rocks
        spawnCoroutine = StartCoroutine(spawnRocksCoroutine());
    }
    private IEnumerator spawnRocksCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRock();
        }
    }
    private void SpawnRock()
    {
        int randomIndex = Random.Range(0, rockPrefabs.Length);
        GameObject rockPrefab = rockPrefabs[randomIndex];
        spawnRotation.y = Random.Range(0, 360); // Randomize the y rotation

        // Instantiate the rock prefab at the spawn position and rotation
        GameObject spawnedRock = Instantiate(rockPrefab, spawnPosition, Quaternion.Euler(spawnRotation));

        // Set the spawned rock's parent to this object
        spawnedRock.transform.parent = transform;

        // Optionally, you can add a Rigidbody component to the spawned rock if needed
        //Rigidbody rb = spawnedRock.AddComponent<Rigidbody>();
        //rb.useGravity = false; // Disable gravity if you want the rock to float
    }
}
