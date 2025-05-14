using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroArea : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    private HashSet<GameObject> targets = new();

    public GameObject GetClosestTarget(Vector3 reference)
    {
        GameObject closest = null;
        float closestDistance = float.PositiveInfinity;
        
        foreach (GameObject target in targets)
        {
            float targetDistance = Vector3.Distance(target.transform.position, reference);
            if (targetDistance < closestDistance)
            {
                closest = target;
                closestDistance = targetDistance;
            }
        }

        return closest;
    }

    public bool HasAggro => targets.Count > 0;

    private void OnTriggerEnter(Collider other)
    {
        // Testando se a layer está entre as layers alvo
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            targets.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Testando se a layer está entre as layers alvo
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            targets.Remove(other.gameObject);
        }
    }
}
