using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    [SerializeField] LayerMask collision;
    [SerializeField] float maxDistance = 40f;
    [SerializeField] Transform origin;
    [SerializeField] GameObject shadowObject;

    private void FixedUpdate()
    {
        Ray ray = new Ray(origin.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, collision))
        {
            shadowObject.SetActive(true);
            shadowObject.transform.position = hit.point + Vector3.up * 0.1f;
        }
        else
        {
            shadowObject.SetActive(false);
        }
    }

}
