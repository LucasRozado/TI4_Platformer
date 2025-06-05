using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingPilar : MonoBehaviour
{
    [SerializeField] private GameObject fallingPilarPrefab;
    [SerializeField] private Rigidbody fallingPilarRigidbody;
    [SerializeField] private Vector3 pilarPosition;
    [SerializeField] private Vector3 pilarRotation;
    [SerializeField] private float fallingForce = 5f;
    [SerializeField] private float fallDelay = 2f;
    private Coroutine fallCoroutine;

    public void Awake()
    {
        fallingPilarRigidbody = fallingPilarPrefab.GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            fallCoroutine = StartCoroutine(FallCoroutine());
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false; // Disable the collider to prevent further triggers
            }
        }
    }
    public void StartFalling()
    {
        Instantiate(fallingPilarPrefab, pilarPosition, Quaternion.Euler(pilarRotation));
        fallingPilarRigidbody.AddForce(Vector3.down * fallingForce, ForceMode.Impulse);
    }
    private IEnumerator FallCoroutine()
    {
        yield return new WaitForSeconds(fallDelay);
        StartFalling();
    }
}
