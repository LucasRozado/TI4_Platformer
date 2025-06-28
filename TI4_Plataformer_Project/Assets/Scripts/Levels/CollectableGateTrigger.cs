using UnityEngine;

public class CollectableGateTrigger : MonoBehaviour
{
    [SerializeField] CollectableType type;
    [SerializeField] int gateValue = 70;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.collectableManager.GetScore(type) >= gateValue)
        {
            ActivateTrigger();
        }
    }

    public void ActivateTrigger()
    {

    }
}
