using UnityEngine;

public class RockSpawnerActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] activeRockSpawner;
    [SerializeField] private GameObject[] inactiveRockSpawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 7 is the layer for the player
        {
            Debug.Log("RockSpawnerActivator in: " + transform.name);
            ActivateRockSpawners();
            DeactivateRockSpawners();
        }
    }
    private void ActivateRockSpawners()
    {
        foreach (GameObject spawner in activeRockSpawner)
        {
            spawner.SetActive(true);
        }
    }
    private void DeactivateRockSpawners()
    {
        foreach (GameObject spawner in inactiveRockSpawner)
        {
            spawner.SetActive(false);
        }
    }

}
