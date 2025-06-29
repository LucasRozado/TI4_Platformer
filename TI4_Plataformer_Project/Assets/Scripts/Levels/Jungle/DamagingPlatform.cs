using UnityEngine;

public class DamagingPlatform : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    [SerializeField] float radius;
    [SerializeField] Transform sphereOrigin;
    [SerializeField] Collider platformCollider;
    Animator animator;
    public void DealDamage()
    {
        platformCollider.enabled = false;
        Collider[] hit = Physics.OverlapSphere(sphereOrigin.position, radius);
        foreach (Collider col in hit)
        {
            if (col.gameObject.TryGetComponent<Player>(out Player player))
            {
                Player.instance.TakeDamage();             
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        delay -= Time.deltaTime;
        animator.SetFloat("Delay", delay);
    }

    public void EnablePlatform()
    {
        platformCollider.enabled = true;
    }
}
