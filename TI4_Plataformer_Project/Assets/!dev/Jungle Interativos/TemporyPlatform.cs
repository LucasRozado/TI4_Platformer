using UnityEngine;

public class TemporyPlatform : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    [SerializeField] Collider platformCollider;
    Animator animator;
    public void DisablePlatform()
    {
        platformCollider.enabled = false;
    }

    public void EnablePlatform()
    {
        platformCollider.enabled = true;
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
}
