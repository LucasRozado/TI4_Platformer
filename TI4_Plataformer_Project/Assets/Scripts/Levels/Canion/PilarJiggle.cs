using UnityEngine;

public class PilarJiggle : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public int jiggleIndex = 0; // Optional index for specific jiggle animations
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 3 is the layer for the player
        {
            Debug.Log("JiggleTrigger in: " + transform.name);
            JiggleAnimation(jiggleIndex);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 3 is the layer for the player
        {
            Debug.Log("JiggleTrigger out: " + transform.name);
            JiggleAnimation(0); // Reset or stop jiggle animation
        }
    }
    void JiggleAnimation(int jiggleIndex)
    {
        // This method can be used to trigger specific jiggle animations if needed
        Debug.Log("Jiggle animation triggered with index: " + jiggleIndex);
        if (animator != null)
        {
            animator.SetInteger("jiggleIndex", jiggleIndex);
        }
    }
}
