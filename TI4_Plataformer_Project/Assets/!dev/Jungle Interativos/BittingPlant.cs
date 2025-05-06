using System.Collections;
using UnityEngine;

public class BittingPlant : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    [SerializeField] float radius;
    [SerializeField] Transform sphereOrigin;
    [SerializeField] LayerMask targets;
    [SerializeField] float maxDistance = 8f;
    [SerializeField] float moveTimer = 0.1f;
    bool hasTarget;
    Vector3 newPosition;
    Vector3 startPosition;
    Animator animator;
    public void DealDamage()
    {
        Collider[] hit = Physics.OverlapSphere(sphereOrigin.position, radius);
        foreach (Collider col in hit)
        {
            if (col.gameObject.TryGetComponent<Player>(out Player player))
            {
                Debug.Log("Player hit");
            }
        }
    }

    public void MeasureDistance()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, radius, out hit, maxDistance, targets))
        {
            hasTarget = true;
            newPosition = startPosition + Vector3.Distance(startPosition, hit.point) * transform.forward;
        }
        else
        {
            newPosition = startPosition + transform.forward * maxDistance;
        }
    }

    public void Move()
    {
        StartCoroutine(CRMove());
    }

    public void Return()
    {
        StartCoroutine(CRReturn());
    }

    public IEnumerator CRReturn()
    {
        float t = 0;
        while (t < moveTimer)
        {
            t += Time.deltaTime;
            Mathf.Clamp(t, 0, moveTimer);
            transform.position = Vector3.Lerp(newPosition, startPosition, t / moveTimer);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CRMove()
    {
        float t = 0;
        while (t < moveTimer)
        {
            t += Time.deltaTime;
            Mathf.Clamp(t, 0, moveTimer);
            transform.position = Vector3.Lerp(startPosition, newPosition, t / moveTimer);
            yield return new WaitForEndOfFrame();
        }
        DealDamage();
        hasTarget = false;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }

    private void Start()
    {
        startPosition = transform.position;
        newPosition = startPosition;
    }

    private void Update()
    {
        delay -= Time.deltaTime;
        animator.SetFloat("Delay", delay);
    }
}
