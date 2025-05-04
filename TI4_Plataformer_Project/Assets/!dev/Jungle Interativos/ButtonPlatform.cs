using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    [SerializeField] ButtonGate gate;
    [SerializeField] Animator animator;
    bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            isActive = true;
            animator.SetBool("IsActive", true);
            gate.GateOpen();
        }
    }
}
