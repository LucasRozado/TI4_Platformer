using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyAggroArea aggroArea;

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

    private Vector3 GetRandomPosition()
    {
        // Calculando uma direcao aleatoria na borda de um cilindro
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();
        randomDirection.y = UnityEngine.Random.value;

        const float minMovement = 4f;
        const float maxMovement = 8f;

        Vector3 offset = transform.rotation * randomDirection * UnityEngine.Random.Range(minMovement, maxMovement);
        Vector3 randomPosition = transform.position + offset;

        return randomPosition;
    }

    private void Idle()
    {
        const float durationMin = 3f;
        const float durationMax = 5f;

        float duration = UnityEngine.Random.Range(durationMin, durationMax);

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
        Vector3 position = GetRandomPosition();
        StartCoroutine(Patrol_Coroutine(position));
    }
    private IEnumerator Patrol_Coroutine(Vector3 position)
    {
        agent.SetDestination(position);

        while (agent.pathPending || Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance)
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

    private void Follow()
    {
        StartCoroutine(Follow_Coroutine());
    }
    private IEnumerator Follow_Coroutine()
    {
        while (aggroArea.HasAggro)
        {
            GameObject target = aggroArea.GetClosestTarget(transform.position);

            if (Vector3.Distance(transform.position, target.transform.position) <= attackDistance)
            {
                Attack();
                yield break;
            }

            agent.SetDestination(target.transform.position);
            yield return null;
        }

        Idle();
    }

    private void Attack()
    {
        const float duration = 1f;
        StartCoroutine(Attack_Coroutine(duration));
    }
    private IEnumerator Attack_Coroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (aggroArea.HasAggro)
        { Follow(); }
        else
        { Idle(); }
    }
}
