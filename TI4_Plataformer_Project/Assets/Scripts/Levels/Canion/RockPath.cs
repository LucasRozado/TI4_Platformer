using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockPath : MonoBehaviour
{
    [SerializeField] private Vector3[] targets;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedRandomizer = 1;
    [SerializeField] private int currentTargetIndex = 0;
    private Rigidbody rb;
    private Coroutine followTargetsCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedRandomizer = Random.Range(0, speedRandomizer);
        rb = GetComponent<Rigidbody>();
        if (targets.Length == 0)
        {
            Debug.LogError("No targets assigned to RockPath script on " + gameObject.name);
            return;
        }
        Debug.Log("RockPathTrigger in: " + transform.name);
        followTargetsCoroutine = StartCoroutine(FollowTargets());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Debug.Log("RockPathTrigger in: " + transform.name);
            DestroyRockPath();
        }
    }
    public void DestroyRockPath()
    {
        Destroy(gameObject, 0.2f); // Destroy the object after 2 seconds
        if (followTargetsCoroutine != null)
        {
            StopCoroutine(followTargetsCoroutine);
            followTargetsCoroutine = null;
        }
    }
    private IEnumerator FollowTargets()
    {
        while (true)
        {
            if (currentTargetIndex < targets.Length)
            {
                Vector3 targetPosition = targets[currentTargetIndex];
                Vector3 direction = (targetPosition - transform.position).normalized;
                rb.MovePosition(transform.position + direction * (speed + speedRandomizer) * Time.fixedDeltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 1f)
                {
                    Debug.Log(currentTargetIndex);
                    currentTargetIndex++;
                }
            }
            else
            {
                yield break; // Exit the coroutine when all targets are reached
            }
            yield return null;
        }
    }
}
