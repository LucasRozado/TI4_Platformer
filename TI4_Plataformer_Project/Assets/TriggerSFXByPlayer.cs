using System;
using UnityEngine;
public class TriggerSFXByPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundToPlay;
    [SerializeField] private float delay = 0f;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool playOnEnter = true;
    [SerializeField] private bool playOnExit = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && playOnEnter == true) // Assuming 3 is the layer for the player
        {
            if (soundToPlay.Length > 0)
            {
                GlobalSound.instance.PlayClip(soundToPlay[UnityEngine.Random.Range(0, soundToPlay.Length)], volume);
            }
            else
            {
                Debug.LogWarning("No sound clips assigned to TriggerSFXByPlayer on " + gameObject.name);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && playOnExit == true) // Assuming 3 is the layer for the player
        {
            if (soundToPlay.Length > 0)
            {
                GlobalSound.instance.PlayClip(soundToPlay[UnityEngine.Random.Range(0, soundToPlay.Length)], volume);
            }
            else
            {
                Debug.LogWarning("No sound clips assigned to TriggerSFXByPlayer on " + gameObject.name);
            }
        }
    }
}
