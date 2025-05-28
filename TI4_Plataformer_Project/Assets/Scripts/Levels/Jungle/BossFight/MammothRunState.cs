using UnityEngine;

public class MammothRunState : BossState
{
    [SerializeField] BossState triggerState;
    [SerializeField] BossState crashState;
    [SerializeField] BossState timerState;
    [SerializeField] float speed;
    public override void Trigger(Collider other)
    {
        base.Trigger(other);        
        if (other.CompareTag("Obstacle"))
        {
            machine.ChangeState(triggerState);
        }
        else if (other.CompareTag("Pillar"))
        {
            machine.ChangeState(crashState);
            Destroy(other.gameObject);
        }
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
        Move();
    }

    public void Move()
    {
        machine.rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (timer <= 0 )
        {
            machine.ChangeState(timerState);
        }
    }
}
