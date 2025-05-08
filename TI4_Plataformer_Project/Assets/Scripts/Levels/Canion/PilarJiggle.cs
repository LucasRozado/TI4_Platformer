using UnityEngine;

public class PilarJiggle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("JiggleTrigger in: " + transform.name);
        }
    }
}
