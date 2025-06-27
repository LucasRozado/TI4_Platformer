using System;
using UnityEngine;
public class TriggerSFXByPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundToPlay;
    [SerializeField] private float delay = 0f;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool playOnEnter = true;
    [SerializeField] private bool playOnExit = false;
    [SerializeField] private bool playOnLoop = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && playOnEnter == true) // Assuming 3 is the layer for the player
        {
            if (soundToPlay.Length > 0)
            {
                if (playOnLoop)
                {
                    GlobalSound.instance.PlayClip(soundToPlay[UnityEngine.Random.Range(0, soundToPlay.Length)], volume, true); // Play the sound in loop
                }
                else
                {
                    GlobalSound.instance.PlayClip(soundToPlay[UnityEngine.Random.Range(0, soundToPlay.Length)], volume);
                }
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
        if (playOnLoop)
        {
            GlobalSound.instance.StopClip(); // Stop the looping sound when the player exits
        }
    }
    void OntriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3 && playOnLoop == true) // Assuming 3 is the layer for the player
        {
            if (soundToPlay.Length > 0)
            {
                GlobalSound.instance.PlayClip(soundToPlay[UnityEngine.Random.Range(0, soundToPlay.Length)], volume, true);
            }
            else
            {
                Debug.LogWarning("No sound clips assigned to TriggerSFXByPlayer on " + gameObject.name);
            }
        }
    }
}
