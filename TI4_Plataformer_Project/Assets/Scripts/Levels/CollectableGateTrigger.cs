using UnityEngine;

public class CollectableGateTrigger : MonoBehaviour
{
    [SerializeField] CollectableType type;
    [SerializeField] int gateValue = 70;
    [SerializeField] ParticleSystem completionVFX;

    private void Start()
    {
        if (GameManager.endGame.GetLevel(type))
        {
            gameObject.SetActive(false);
            ActivateTrigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.collectableManager.GetScore(type) >= gateValue)
        {
            ActivateTrigger();
        }
    }

    public void ActivateTrigger()
    {
        GameManager.endGame.CompleteLevel(type);
        completionVFX.gameObject.SetActive(true);
    }
}
