using UnityEngine;

public class MammothDeathState : BossState
{
    [SerializeField] ButtonActivated[] activated;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] int intReference;
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Death();
    }

    public void Death()
    {
        GameManager.powerUp.AcquirePowerUp(PowerUps.Push);
        Destroy(gameObject);
        foreach (ButtonActivated act in activated)
        {
            act.Activate();
        }
        LevelProgress.instance.Activate(intReference);
        AudioManager.PlayMusicLoop(levelMusic);
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
