using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FarusPatrol : BossState
{
    [SerializeField] Transform[] waypointSequence;
    int waypointI = 0;
    [SerializeField] float speed;
    [SerializeField] AimTarget target;

    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
        target.TargetBase();
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
        Vector3 direction = waypointSequence[waypointI].position - transform.position;
        Debug.DrawLine(waypointSequence[waypointI].position, transform.position, Color.red);
        machine.animator.transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
        if (Vector3.Magnitude(direction) <= 0.1f)
        {
            waypointI++;
            waypointI %= waypointSequence.Length;
        }
    }
}
