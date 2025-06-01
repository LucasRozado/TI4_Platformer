using UnityEngine;

public class FinalWaypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            Destroy(other.gameObject);
        }
    }
}
