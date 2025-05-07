using UnityEngine;

public class VoidTrigger : MonoBehaviour
{
    [SerializeField] CPInteraction firstCheckpoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            GameManager.Instance.ResetToCheckPoint();
        }
    }
}
