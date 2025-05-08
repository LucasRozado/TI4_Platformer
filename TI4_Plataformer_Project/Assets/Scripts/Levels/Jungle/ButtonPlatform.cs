using System;
using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    [SerializeField] ButtonActivated[] activated;
    [SerializeField] Animator animator;
    bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            isActive = true;
            if (animator != null)
            {
                animator.SetBool("IsActive", true);
            }
            foreach (ButtonActivated act in activated)
            {
                act.Activate();
            }
        }
    }
}
