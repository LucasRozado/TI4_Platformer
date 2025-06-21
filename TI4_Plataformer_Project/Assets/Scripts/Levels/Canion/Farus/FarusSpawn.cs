using UnityEngine;

public class FarusSpawn : BossState
{
    [SerializeField] FarusPatrol patrolState;
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        machine.ChangeState(patrolState);
    }
}
