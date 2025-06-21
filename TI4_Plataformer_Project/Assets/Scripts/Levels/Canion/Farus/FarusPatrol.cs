using UnityEngine;

public class FarusPatrol : BossState
{
    [SerializeField] Transform[] waypointSequence;
    int waypointI = 0;
    [SerializeField] float speed;
    public override void StateUpdate()
    {
        base.StateUpdate();
        Vector3 direction = waypointSequence[waypointI].position - transform.position;
        Debug.DrawLine(waypointSequence[waypointI].position, transform.position, Color.red);
        transform.rotation = Quaternion.LookRotation(direction.normalized);
        transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
        if (Vector3.Magnitude(direction) <= 0.1f)
        {
            waypointI++;
            waypointI %= waypointSequence.Length;
        }
    }
}
