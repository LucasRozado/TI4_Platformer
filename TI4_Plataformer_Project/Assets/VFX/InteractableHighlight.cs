using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    ParticleSystem pS;
    Transform parent;

    private void Awake()
    {
        pS = GetComponent<ParticleSystem>();
        pS.Stop();
    }
    private void Start()
    {
        var shape = pS.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        parent = transform.parent;
        shape.meshRenderer = parent.GetComponent<MeshRenderer>();
        //shape.scale = parent.localScale * 1.2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pS.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pS.Stop();
        }
    }
}
