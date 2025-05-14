using UnityEngine;

public class PilarJiggle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("JiggleTrigger in: " + transform.name);
        }
    }
}
