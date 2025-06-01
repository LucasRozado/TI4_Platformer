using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private bool isActive = true;
    private Rigidbody rb;
    private Coroutine moveCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Assuming 7 is the layer for the player
        {
            Debug.Log("ElevatorTrigger in: " + transform.name);
            isActive = true;
            moveCoroutine = StartCoroutine(MoveElevator());
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == 3) // Assuming 7 is the layer for the player
        {
            StopCoroutine(moveCoroutine);
            Debug.Log("ElevatorTrigger out: " + transform.name);
            isActive = false;
            moveCoroutine = StartCoroutine(MoveElevator());
        }
    }
    private IEnumerator MoveElevator()
    {
        yield return new WaitForSeconds(waitTime);
        if (isActive)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                    if (targetPosition.y > transform.position.y)
                    {
                        rb.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
                    }
                    else
                    {
                        rb.AddForce(Vector3.down * speed, ForceMode.VelocityChange);
                    }
                yield return null;
            }
        }

        // Wait for a while
        yield return new WaitForSeconds(waitTime);

        // Move back to start position
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            if (!isActive)
            {
                if (startPosition.y > transform.position.y)
                {
                    rb.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
                }
                else
                {
                    rb.AddForce(Vector3.down * speed, ForceMode.VelocityChange);
                }
            }
            yield return null;
        }
    }
}
