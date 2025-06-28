using UnityEngine;

public class VoidTrigger : MonoBehaviour
{
    [SerializeField] CPInteraction firstCheckpoint;
    
    [SerializeField, Tooltip("In seconds")]
    private float respawnDelay = 2f; // Delay before respawning the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.Die();
            Player.instance.GetComponent<PlayerState_Dead>().SetDuration(respawnDelay);
        }
    }
}
