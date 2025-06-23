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
            case 4: // Idle Platform 5
                animator.SetBool("idlePlatform4", true);
                break;
            case 5: // Idle Platform 6
                animator.SetBool("idlePlatform5", true);
                break;
            case 6: // Idle Platform 7
                animator.SetBool("idlePlatform6", true);
                break;
            case 7: // Idle Platform 8
                animator.SetBool("idlePlatform7", true);
                break;
            case 8: // Idle Platform 9
                animator.SetBool("idlePlatform8", true);
                break;
            case 9: // Idle Platform 10
                animator.SetBool("idlePlatform9", true);
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
                case 4: // Idle Platform 5
                    animator.SetBool("idlePlatform4", false);
                    break;
                case 5: // Idle Platform 6
                    animator.SetBool("idlePlatform5", false);
                    break;
                case 6: // Idle Platform 7
                    animator.SetBool("idlePlatform6", false);
                    break;
                case 7: // Idle Platform 8
                    animator.SetBool("idlePlatform7", false);
                    break;
                case 8: // Idle Platform 9
                    animator.SetBool("idlePlatform8", false);
                    break;
                case 9: // Idle Platform 10
                    animator.SetBool("idlePlatform9", false);
                    break;
                case 10: // Idle Platform 11
                    animator.SetTrigger("jigglePlatform");
                    break;
                case 11: // Idle Platform 12
                    animator.SetTrigger("jigglePlatform1");
                    break;
                case 12: // Idle Platform 13
                    animator.SetTrigger("jigglePlatform2");
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
                case 4: // Idle Platform 5
                    animator.SetBool("idlePlatform4", true);
                    break;
                case 5: // Idle Platform 6
                    animator.SetBool("idlePlatform5", true);
                    break;
                case 6: // Idle Platform 7
                    animator.SetBool("idlePlatform6", true);
                    break;
                case 7: // Idle Platform 8
                    animator.SetBool("idlePlatform7", true);
                    break;
                case 8: // Idle Platform 9
                    animator.SetBool("idlePlatform8", true);
                    break;
                case 9: // Idle Platform 10
                    animator.SetBool("idlePlatform9", true);
                    break;
                default:
                    Debug.LogWarning("Unknown platform type");
                    break;
            }
        }
    }
}
