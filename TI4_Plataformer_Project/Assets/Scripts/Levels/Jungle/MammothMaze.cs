using UnityEngine;

public class MammothMaze : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform firstWaypoint;
    [SerializeField] Vector3 currentWaypoint;
    [SerializeField] float speed = 6f;
    [SerializeField] float baseSpeed = 6f;
    public bool isFinal;
    [Header("Charge")]
    [SerializeField] float radius = 3.5f;
    [SerializeField] LayerMask player;
    [SerializeField] float chargeSpeed = 13f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentWaypoint = firstWaypoint.position;
        speed = baseSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        DetectPlayer();
    }

    public void DetectPlayer()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, radius, Vector3.Distance(transform.position, currentWaypoint), player))
        {
            speed = chargeSpeed;
        }
    }

    private void Move()
    {
        Vector3 direction = currentWaypoint - transform.position;
        rb.MovePosition(transform.position + direction.normalized * Time.fixedDeltaTime * speed);
    }

    public void ChangeWaypoint(Vector3 newWaypoint)
    {
        speed = baseSpeed;
        currentWaypoint = newWaypoint;
        transform.LookAt(currentWaypoint);
    }

    public Vector3 GetLastWaypoint()
    {
        return currentWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by mammoth");
        }
    }
}
