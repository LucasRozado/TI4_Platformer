using UnityEngine;

public class MovingTrigger : MonoBehaviour
{
    [SerializeField] MovingPlatform movingPlatform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent == null)
        {
            Debug.Log("Foi");
            other.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent = transform)
        {
            other.gameObject.transform.parent = null;
            movingPlatform.player = null;
        }
    }
}
