using UnityEngine;

public class MammothDeathState : BossState
{
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Death();
    }

    public void Death()
    {
        GameManager.powerUp.AcquirePowerUp(PowerUps.Push);
        Destroy(gameObject);
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
