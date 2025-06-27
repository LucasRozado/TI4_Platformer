using UnityEngine;

public class FarusActivator : MonoBehaviour
{
    public GameObject farus;
    public Animator farusAnimator;
    void Start()
    {
        if (farus == null)
        {
            Debug.LogError("Farus GameObject is not assigned in the inspector.");
        }
        else
        {
            farus.SetActive(false);
            farusAnimator = farus.GetComponentInChildren<Animator>();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 3 is the layer for the player
        {
            Debug.Log("FARUS ACTIVATED");
            farus.SetActive(true);
            farusAnimator.SetBool("Patrol", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 3 is the layer for the player
        {
            Debug.Log("FARUS DEACTIVATED");
            farus.SetActive(false);
        }
    }
}
