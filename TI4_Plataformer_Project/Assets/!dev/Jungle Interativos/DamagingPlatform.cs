using UnityEngine;

public class DamagingPlatform : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] Transform sphereOrigin;
    public void DealDamage()
    {
        Collider[] hit = Physics.OverlapSphere(sphereOrigin.position, radius);
        foreach (Collider col in hit)
        {
            if (TryGetComponent<Player>(out Player player))
            {
                Debug.Log("Player hit");
            }
        }
    }
}
