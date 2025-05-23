using System;
using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    [SerializeField] LevelProgress level;
    [SerializeField] int levelBool;
    [SerializeField] ButtonActivated[] activated;
    [SerializeField] Animator animator;
    bool isActive;

    private void OnEnable()
    {
        if (level.levelProgress[levelBool])
        {
            Activate();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
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
        level.levelProgress[levelBool] = true;
    }
}
