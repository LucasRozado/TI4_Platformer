using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FarusSpawn : BossState
{
    [SerializeField] FarusPatrol patrolState;
    [SerializeField] RigBuilder rigBuilder;

    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
        rigBuilder.enabled = false;
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        machine.ChangeState(patrolState);
        rigBuilder.enabled = true;
    }
}
