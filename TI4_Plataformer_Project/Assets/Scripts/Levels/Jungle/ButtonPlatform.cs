using System;
using UnityEngine;

public class ButtonPlatform : Progress
{
    [SerializeField] ButtonActivated[] activated;
    [SerializeField] Animator animator;
    bool isActive;

    private void Start()
    {
        if (LevelProgress.instance.GetProgress(intReference))
        {
            Debug.Log($"{intReference} is active");
            ManualActivate();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void Activate()
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
        LevelProgress.instance.Activate(intReference);
    }

    private void ManualActivate()
    {
        isActive = true;
        if (animator != null)
        {
            animator.SetBool("IsActive", true);
        }
        foreach (ButtonActivated act in activated)
        {
            act.ManualActivation();
        }
        LevelProgress.instance.Activate(intReference);
    }
}
