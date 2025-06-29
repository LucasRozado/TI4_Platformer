using UnityEngine;

public class MammothRoarState : BossState
{
    [SerializeField] BossState animationState;
    [SerializeField] AudioClip roarSound;
    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
        GlobalSound.instance.PlayClip(roarSound);
    }

    public override void ExitState(BossMachine machine)
    {
        base.ExitState(machine);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        Vector3 direction = Player.instance.transform.position - transform.position;
        machine.rb.MoveRotation(Quaternion.LookRotation(direction.normalized));
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        
    }

    public override void Trigger(Collider other)
    {
        base.Trigger(other);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        machine.ChangeState(animationState);
    }
}
