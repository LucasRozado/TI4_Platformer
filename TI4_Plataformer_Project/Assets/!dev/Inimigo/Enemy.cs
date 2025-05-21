using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAggroArea aggroArea;

    [Header("Idle")]
    [SerializeField] private float idleDurationMin = 3f;
    [SerializeField] private float idleDurationMax = 5f;

    [Header("Patrol")]
    [SerializeField] private float patrolDistanceMin = 4f;
    [SerializeField] private float patrolDistanceMax = 8f;

    [Header("Attack")]
    [SerializeField] private float attackDistance;

    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Idle();
    }

    private void Idle()
    {
        float duration = UnityEngine.Random.Range(idleDurationMin, idleDurationMax);

        StartCoroutine(Idle_Coroutine(duration));
    }
    private IEnumerator Idle_Coroutine(float duration)
    {
        while (duration > 0)
        {
            if (aggroArea.HasAggro)
            {
                Follow();
                yield break;
            }

            duration -= Time.deltaTime;
            yield return null;
        }

        Patrol();
    }

    private void Patrol()
    {
        Vector3 position = GetRandomPatrolPoint();
        StartCoroutine(Patrol_Coroutine(position));
    }
    private IEnumerator Patrol_Coroutine(Vector3 position)
    {
        agent.SetDestination(position);
        
        // Tratando para nao preferir ficar nos cantos
        while (agent.pathPending)
        { yield return null; }
        if (Vector3.Distance(agent.destination, transform.position) < patrolDistanceMin)
        {
            Debug.Log("Recalculating patrol");
            Patrol();
            yield break;
        }

        while (Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance)
        {
            if (aggroArea.HasAggro)
            {
                Follow();
                yield break;
            }

            yield return null;
        }

        Idle();
    }
    private Vector3 GetRandomPatrolPoint()
    {
        // Calculando uma direcao aleatoria na superficie de um tubo
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();
        randomDirection.y = UnityEngine.Random.value;

        Vector3 offset = transform.rotation * randomDirection * UnityEngine.Random.Range(patrolDistanceMin, patrolDistanceMax);
        Vector3 randomPosition = transform.position + offset;

        return randomPosition;
    }

    private void Follow()
    {
        StartCoroutine(Follow_Coroutine());
    }
    private IEnumerator Follow_Coroutine()
    {
        while (aggroArea.HasAggro)
        {
            Player player = aggroArea.Target.GetComponent<Player>();

            Vector3 directionVector = player.transform.position - transform.position;
            if (directionVector.magnitude <= attackDistance)
            {
                agent.SetDestination(transform.position);
                Quaternion direction = Quaternion.LookRotation(directionVector, transform.up);
                Attack(direction);
                yield break;
            }

            //Vector3 targetDistance = target.transform.position - transform.position;
            //Vector3 attackPosition = targetDistance * (targetDistance.magnitude - attackDistance);
            agent.SetDestination(player.transform.position);
            yield return null;
        }

        Idle();
    }

    private void Attack(Quaternion direction)
    {
        const float duration = 1f;
        StartCoroutine(Attack_Coroutine(direction, duration));
    }
    private IEnumerator Attack_Coroutine(Quaternion direction, float duration)
    {
        while (duration > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, Time.deltaTime * 5f);

            duration -= Time.deltaTime;
            yield return null;
        }

        if (aggroArea.HasAggro)
        { Follow(); }
        else
        { Idle(); }
    }
}
