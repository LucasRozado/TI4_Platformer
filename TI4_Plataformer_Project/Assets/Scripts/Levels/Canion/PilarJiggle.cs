using UnityEngine;

public class PilarJiggle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 3 is the layer for the player
        {
            Debug.Log("JiggleTrigger in: " + transform.name);
        }
    }
}
