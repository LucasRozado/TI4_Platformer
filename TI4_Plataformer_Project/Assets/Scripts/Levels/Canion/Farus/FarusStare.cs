using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FarusStare : BossState
{
    [SerializeField] FarusPatrol patrolState;
    [SerializeField] float slow = 0.5f;
    [SerializeField] Transform bone;
    [SerializeField] RigTransform rigTransform;
    [Tooltip("Player and Obstacles")]
    [SerializeField] LayerMask rayInteract;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rayDistance;
    [SerializeField] float damageDelay = 1f;
    float damageTimer = 0;
    [SerializeField] float cancelDelay = 1f;
    float cancelTimer = 0;
    [SerializeField] LineRenderer line;

    public override void EnterState(BossMachine machine)
    {
        base.EnterState(machine);
        rigTransform.enabled = true;
    }

    public override void ExitState(BossMachine machine)
    {
        base.ExitState(machine);
        rigTransform.enabled = false;
        damageTimer = 0f;
        cancelTimer = 0f;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        bone.LookAt(Player.instance.transform.position);
        line.SetPosition(0, bone.position);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        CheckPlayerVisible();
    }

    private void CheckPlayerVisible()
    {
        Debug.DrawRay(bone.position, (Player.instance.transform.position + Vector3.up) - bone.position);
        RaycastHit hit;
        if (Physics.Raycast(bone.position, (Player.instance.transform.position + Vector3.up) - bone.position,
            out hit, rayDistance, rayInteract, QueryTriggerInteraction.Ignore))
        {            
            if (Physics.Raycast(bone.position, (Player.instance.transform.position + Vector3.up) - bone.position,
            out hit, rayDistance, playerLayer, QueryTriggerInteraction.Ignore))
            {
                line.SetPosition(1, hit.point);
                Debug.Log("Hit player");
                HitPlayer();
            }
            else
            {
                Debug.Log("Hit obstacle");
                WaitReset();
            }
        }
    }

    private void WaitReset()
    {
        Player.instance.ChangeSpeed(1);
        damageTimer = 0f;
        cancelTimer += Time.fixedDeltaTime;
        Debug.Log("damage timer " + damageTimer);
        Debug.Log("wait timer " + cancelTimer);
        if (cancelTimer >= cancelDelay)
        {
            machine.ChangeState(patrolState);
        }
    }

    private void HitPlayer()
    {
        Player.instance.ChangeSpeed(slow);
        cancelTimer = 0f;
        damageTimer += Time.fixedDeltaTime;
        Debug.Log("damage timer " + damageTimer);
        Debug.Log("wait timer " + cancelTimer);
        if (damageTimer >= damageDelay)
        {
            damageTimer = 0f;
            Player.instance.TakeDamage();
            machine.ChangeState(patrolState); //verificar inconsistencia
        }
    }
}
