using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found in children.");
        }
    }
    public void WalkAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", state);
        }
    }
    public void JumpAnimation(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("jumpType", state);
        }
    }
    public void AirbourneAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("isAirbourne", state);
        }
    }
    public void ClimbAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("isClimbing", state);
        }
    }
    public void ClimbTypeAnimation(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("climbType", state);
        }
    }
    public void HoldAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("isHolding", state);
        }
    }
    public void HoldTypeAnimation(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("holdType", state);
        }
    }
    public void RunningAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", state);
        }
    }
    public void RunningSpeedAnimation(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("runningSpeed", speed);
        }
    }
    public void SwimAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("isSwimming", state);
        }
    }
    public void SwimSpeedAnimation(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("swimSpeed", speed);
        }
    }
}   
