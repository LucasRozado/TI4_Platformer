using UnityEngine;

public class CollectableGateTrigger : MonoBehaviour
{
    [SerializeField] ButtonActivated gate;
    [SerializeField] CollectableType type;
    [SerializeField] int gateValue = 70;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.collectableManager.GetScore(type) >= gateValue)
        {
            gate.Activate();
        }
    }
}
