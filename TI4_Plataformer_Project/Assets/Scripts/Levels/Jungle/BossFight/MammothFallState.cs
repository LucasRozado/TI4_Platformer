using UnityEngine;

public class MammothFallState : BossState
{
    [SerializeField] BossState animationState;
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        machine.ChangeState(animationState);
    }

    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
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
