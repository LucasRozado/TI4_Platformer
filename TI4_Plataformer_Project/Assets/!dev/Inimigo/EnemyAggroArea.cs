using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroArea : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;

    private GameObject target;

    public bool HasAggro => target != null;
    public GameObject Target => target;

    private void OnTriggerEnter(Collider other)
    {
        // Testando se a layer está entre as layers alvo
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Testando se a layer está entre as layers alvo
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            target = other.gameObject;
        }
    }
}
