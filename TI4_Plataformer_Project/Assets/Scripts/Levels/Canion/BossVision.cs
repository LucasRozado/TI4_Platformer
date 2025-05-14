using UnityEngine;

public class BossVision : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            Debug.LogError("No colliders assigned to BossVision script on " + gameObject.name);
            return;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
