using UnityEditor.Animations;
using UnityEngine;

public class MammothDamageState : BossState
{
    [SerializeField] BossState animationState;
    [SerializeField] BossState deathState;
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        machine.ChangeState(animationState);
    }

    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
        machine.TakeDamage();
        if (machine.GetHealth() <= 0)
        {
            machine.ChangeState(deathState);
        }
    }

    public override void ExitState(BossMachine machine)
    {
        base.ExitState(machine);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void Trigger(Collider other)
    {
        base.Trigger(other);
    }
}
