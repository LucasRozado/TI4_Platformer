using UnityEngine;

public class FarusActivator : MonoBehaviour
{
    public GameObject farus;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 3 is the layer for the player
        {
            Debug.Log("FARUS ACTIVATED");
            farus.SetActive(true);
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
