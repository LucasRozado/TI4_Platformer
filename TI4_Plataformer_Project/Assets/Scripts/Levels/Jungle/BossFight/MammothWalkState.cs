using UnityEditor;
using UnityEngine;

public class MammothWalkState : BossState
{
    [SerializeField] float midDuration = 0.5f;
    [SerializeField] float speed;
    [SerializeField] Transform[] targets;
    [SerializeField] float targetRadius;
    [SerializeField] BossState timedState;
    float t;
    Transform currentTarget;
    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        Move();
    }

    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
        currentTarget = targets[Random.Range(0, targets.Length)];
        timer += Random.Range(-midDuration, midDuration);
        t = 0;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (timer <= 0)
        {
            machine.ChangeState(timedState);
        }
    }

    public void Move()
    {
        t += Time.deltaTime;
        Debug.Log(t);
        float x = currentTarget.position.x + targetRadius * Mathf.Cos(t * 6.28f);
        float z = currentTarget.position.z + targetRadius * Mathf.Sin(t * 6.28f);

        Vector3 direction = new Vector3(x, transform.position.y, z) - transform.position;
        machine.rb.MovePosition(transform.position + direction.normalized * speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.LookRotation(direction.normalized);
    }
}
