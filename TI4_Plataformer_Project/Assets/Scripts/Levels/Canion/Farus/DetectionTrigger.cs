using Unity.VisualScripting;
using UnityEngine;

public class DetectionTrigger : MonoBehaviour
{
    [SerializeField] FarusMachine machine;
    [SerializeField] Transform head;
    [SerializeField] FarusStare stareState;
    [SerializeField] LayerMask obstacles;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found");
            if (!Physics.Raycast(head.position, head.position - Player.instance.transform.position + Vector3.up,
                Vector3.Distance(head.position, Player.instance.transform.position) + 0.5f, obstacles))
            {
                Debug.Log("Player hit by ray");
                machine.PlayerFound();
            }
        }
    }
}
