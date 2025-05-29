using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] BossMachine machine;
    
    public void AnimTrigger()
    {
        machine.GetCurrentState().AnimationTrigger();
    }
}
