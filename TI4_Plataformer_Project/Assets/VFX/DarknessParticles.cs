using System.Collections.Generic;
using UnityEngine;

public class DarknessParticle : MonoBehaviour
{
    ParticleSystem particles;
    [SerializeField] float transparency = 0.3f;
    [SerializeField] Color main;
    [SerializeField] Color transparent;

    private void Awake()
    {

        particles = GetComponent<ParticleSystem>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles.trigger.SetCollider(0, Player.instance.torchTrigger);
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enterList = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exitList = new List<ParticleSystem.Particle>();
        int enter = particles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterList);
        int exit = particles.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitList);
        for (int i = 0; i < enter; i++)
        {
            ParticleSystem.Particle p = enterList[i];
            p.startColor = p.startColor * new Color(1, 1, 1, transparency);
            enterList[i] = p;
            Debug.Log("Enter");
        }
        for (int i = 0; i < exit; i++)
        {
            ParticleSystem.Particle p = exitList[i];
            p.startColor = p.startColor + new Color(0, 0, 0, 1 - transparency);
            exitList[i] = p;
            Debug.Log("Exit");
        }

        particles.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterList);
        particles.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitList);
    }
}
