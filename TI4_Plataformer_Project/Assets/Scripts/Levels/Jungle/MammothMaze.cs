using UnityEngine;
using UnityEngine.AI;

public class MammothMaze : Progress
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform firstWaypoint;
    [SerializeField] Transform finalWaypoint;
    [SerializeField] Transform bossFightWaypoint;
    [SerializeField] Transform[] waypoints;
    [SerializeField] Vector3 currentWaypoint;
    [SerializeField] float baseSpeed = 6f;
    [SerializeField] float rbSpeed;
    public bool firstGate;
    public bool secondGate;
    bool isEndPosition;
    [Header("Charge")]
    [SerializeField] float maxDistance = 20f;
    [SerializeField] float radius = 3.5f;
    [SerializeField] LayerMask targets;
    [SerializeField] LayerMask player;
    [SerializeField] float chargeSpeed = 13f;
    bool isCharging;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentWaypoint = firstWaypoint.position;
        agent.SetDestination(currentWaypoint);
        //gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (levelProgress.GetProgress(intReference))
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isEndPosition)
        {
            Move();
        }
        
        DetectPlayer();
    }

    public void DetectPlayer()
    {
        if (!isCharging)
        {
            Ray ray = new Ray(transform.position + Vector3.up * radius, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, radius, out hit, maxDistance, targets))
            {
                float barrierDistance = 1 + Vector3.Distance(transform.position + Vector3.up * radius, hit.point);
                if (Physics.SphereCast(ray, radius, barrierDistance, player))
                {
                    if (currentWaypoint == finalWaypoint.position)
                    {
                        currentWaypoint = bossFightWaypoint.position;
                    }
                    ReadyCharge();
                }
            }
        }        
    }

    public void ReadyCharge()
    {
        Debug.Log("Ready Charge");
        isCharging = true;
        animator.SetBool("Charge", isCharging);
        agent.speed = 0;
        rbSpeed = 0;
    }

    public void BeginCharge()
    {
        Debug.Log("Begin Charge");
        agent.speed = chargeSpeed;
        rbSpeed = chargeSpeed;
    }

    public void EndCharge()
    {
        Debug.Log("End Charge");
        rbSpeed = baseSpeed;
        isCharging = false;
        animator.SetBool("Charge", isCharging);
        agent.speed = baseSpeed;
        agent.velocity = Vector3.zero;
    }

    private void Move()
    {
        Vector3 direction = currentWaypoint - transform.position;
        rb.MovePosition(transform.position + direction.normalized * Time.fixedDeltaTime * rbSpeed);
        transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public void ChangeWaypoint()
    {
        if (currentWaypoint != bossFightWaypoint.position)
        {
            EndCharge();
        }
        if (firstGate && secondGate)
        {
            if (!isEndPosition)
            {
                isEndPosition = true;
                agent.enabled = false;
            }
            LineMovementChangeDirection();
            return;
        }
        currentWaypoint = GetRandomWaypoint();
        agent.SetDestination(currentWaypoint);
        
    }

    public void LineMovementChangeDirection()
    {
        if (currentWaypoint == firstWaypoint.position)
        {
            currentWaypoint = finalWaypoint.position;
        }
        else if (currentWaypoint == finalWaypoint.position)
        {
            currentWaypoint = firstWaypoint.position;
        }
    }

    public Vector3 GetRandomWaypoint()
    {
        int random;
        do
        {
            random = Random.Range(0, waypoints.Length);
        }
        while (waypoints[random].position == currentWaypoint);
        return waypoints[random].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by mammoth");
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (other.gameObject.TryGetComponent<MazeTrunk>(out MazeTrunk trunk))
            {
                trunk.OnBossPassage();
            }
        }
    }
    private void OnDrawGizmos()
    {
        foreach (Transform t in waypoints)
        {
            Gizmos.DrawLine(transform.position, t.position);
        }
        if (currentWaypoint != null)
        {
            Gizmos.DrawSphere(currentWaypoint, 1);
        }
    }

    public void EndMaze()
    {
        foreach (Transform t in waypoints)
        {
            t.gameObject.SetActive(false);
        }
        firstWaypoint.gameObject.SetActive(true);
        finalWaypoint.gameObject.SetActive(true);
        currentWaypoint = firstWaypoint.position;
        agent.SetDestination(currentWaypoint);
    }
}
