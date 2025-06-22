using UnityEngine;

public class TriggerAnimationByPlayer : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int platformType = 0;
    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        switch (platformType)
        {
            case 0: // Idle Platform
                animator.SetBool("idlePlatform", true);
                break;
            case 1: // Idle Platform 2
                animator.SetBool("idlePlatform1", true);
                break;
            case 2: // Idle Platform 3
                animator.SetBool("idlePlatform2", true);
                break;
            case 3: // Idle Platform 4
                animator.SetBool("idlePlatform3", true);
                break;
            default:
                Debug.LogWarning("Unknown platform type");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            switch (platformType)
            {
                case 0: // Idle Platform
                    animator.SetBool("idlePlatform", false);
                    break;
                case 1: // Idle Platform 2
                    animator.SetBool("idlePlatform1", false);
                    break;
                case 2: // Idle Platform 3
                    animator.SetBool("idlePlatform2", false);
                    break;
                case 3: // Idle Platform 4
                    animator.SetBool("idlePlatform3", false);
                    break;
                default:
                    Debug.LogWarning("Unknown platform type");
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            switch (platformType)
            {
                case 0: // Idle Platform
                    animator.SetBool("idlePlatform", true);
                    break;
                case 1: // Idle Platform 2
                    animator.SetBool("idlePlatform1", true);
                    break;
                case 2: // Idle Platform 3
                    animator.SetBool("idlePlatform2", true);
                    break;
                case 3: // Idle Platform 4
                    animator.SetBool("idlePlatform3", true);
                    break;
                default:
                    Debug.LogWarning("Unknown platform type");
                    break;
            }
        }
    }
}
