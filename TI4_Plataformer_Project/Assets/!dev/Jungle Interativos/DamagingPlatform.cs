using UnityEngine;

public class DamagingPlatform : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] Transform sphereOrigin;
    [SerializeField] Collider platformCollider;
    public void DealDamage()
    {
        platformCollider.enabled = false;
        Collider[] hit = Physics.OverlapSphere(sphereOrigin.position, radius);
        foreach (Collider col in hit)
        {
            if (col.gameObject.TryGetComponent<Player>(out Player player))
            {
                Debug.Log("Player hit");
            }
        }
    }

    public void EnablePlatform()
    {
        platformCollider.enabled = true;
    }
}
