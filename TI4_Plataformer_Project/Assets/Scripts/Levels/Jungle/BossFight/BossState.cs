using UnityEditor.Animations;
using UnityEngine;

public class BossState : MonoBehaviour
{
    [SerializeField] protected float stateTimer;
    protected float timer;
    protected BossMachine machine;
    [SerializeField] protected string stateName;
    public virtual void EnterState(BossMachine machine)
    {
        this.machine = machine;
        machine.animator.SetBool(stateName, true);
        timer = stateTimer;
    }

    public virtual void ExitState(BossMachine machine)
    {
        machine.animator.SetBool(stateName, false);
    }

    public virtual void StateFixedUpdate()
    {
        
    }

    public virtual void StateUpdate()
    {
        timer -= Time.deltaTime;
    }

    public virtual void Trigger(Collider other)
    {

    }
    public virtual void AnimationTrigger()
    {
        
    }
}
