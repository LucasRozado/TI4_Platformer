using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossVisionCollider : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12) // Assuming 12 is the layer for the barrier
        {
            Debug.Log("BossVisionCollider in: " + transform.name);
            DeactivateCollider();
        }
        if (other.gameObject.layer == 7) // Assuming 7 is the layer for the player
        {
            Debug.Log("player in: " + transform.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12) // Assuming 12 is the layer for the barrier
        {
            Debug.Log("BossVisionCollider out: " + transform.name);
            ActivateCollider();
        }
        if (other.gameObject.layer == 7) // Assuming 7 is the layer for the player
        {
            Debug.Log("player in: " + transform.name);
        }
    }
    private void DeactivateCollider()
    {
        isActive = false;
        Debug.Log("Collider deactivated: " + gameObject.name);
    }
    private void ActivateCollider()
    {
        isActive = true;
        Debug.Log("Collider activated: " + gameObject.name);
    }
}
