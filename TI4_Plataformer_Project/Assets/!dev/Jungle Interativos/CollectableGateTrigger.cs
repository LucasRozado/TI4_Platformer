using UnityEngine;

public class CollectableGateTrigger : MonoBehaviour
{
    [SerializeField] ButtonActivated gate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gate.Activate();
        }
    }
}
