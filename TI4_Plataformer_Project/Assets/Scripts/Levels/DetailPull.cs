using System.Collections.Generic;
using UnityEngine;

public class DetailPull : MonoBehaviour
{
    [Header("Prefab Settings")]
    public List<GameObject> prefabsToSpawn;
    public float spawnChance = 0.7f;
    [Range(0f, 360f)] public float maxYRotationVariation = 30f;

    [Header("Spacing Settings")]
    public float minSpacing = 0.8f;
    public float maxSpacing = 1.2f;
    public bool randomizeSpacing = true;
    public float positionOffset = 0.5f;

    [Header("Raycast Settings")]
    public float raycastDistance = 10f;
    public LayerMask groundLayer;
    public Vector3 raycastOriginOffset = Vector3.zero;
    public bool alignToSurface = true;

    [Header("Debug")]
    public bool drawDebugRays = true;
    public bool drawSpawnPoints = true;
    public Color spawnPointColor = Color.cyan;

    void Start()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Count == 0)
        {
            Debug.LogWarning("Nenhum prefab atribuído para spawnar.");
            return;
        }

        SpreadPrefabs();
    }

    void SpreadPrefabs()
    {
        Collider boxCollider = GetComponent<Collider>();
        if (boxCollider == null)
        {
            Debug.LogError("Nenhum colisor encontrado no objeto.");
            return;
        }

        Bounds bounds = boxCollider.bounds;
        Vector3 currentPosition = bounds.min;

        while (currentPosition.x <= bounds.max.x)
        {
            currentPosition.z = bounds.min.z;

            while (currentPosition.z <= bounds.max.z)
            {
                Vector3 rayStart = new Vector3(
                    currentPosition.x + raycastOriginOffset.x,
                    bounds.max.y + positionOffset + raycastOriginOffset.y,
                    currentPosition.z + raycastOriginOffset.z
                );

                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
                {
                    if (drawSpawnPoints)
                    {
                        Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.2f, spawnPointColor, 5f);
                    }

                    if (Random.value <= spawnChance)
                    {
                        SpawnPrefabAtPoint(hit.point, hit.normal);
                    }
                }

                if (drawDebugRays)
                {
                    Debug.DrawRay(rayStart, Vector3.down * raycastDistance, Color.green, 2f);
                }

                float spacingZ = randomizeSpacing ? Random.Range(minSpacing, maxSpacing) : maxSpacing;
                currentPosition.z += spacingZ;
            }

            float spacingX = randomizeSpacing ? Random.Range(minSpacing, maxSpacing) : maxSpacing;
            currentPosition.x += spacingX;
        }
    }

    void SpawnPrefabAtPoint(Vector3 position, Vector3 surfaceNormal)
    {
        if (prefabsToSpawn.Count == 0) return;

        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];

        // Rotação baseada na normal da superfície
        Quaternion surfaceRotation = Quaternion.identity;
        if (alignToSurface)
        {
            surfaceRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        }

        // Rotação aleatória apenas no eixo Y
        float randomYRotation = Random.Range(-maxYRotationVariation, maxYRotationVariation);
        Quaternion yRotation = Quaternion.Euler(0f, randomYRotation, 0f);

        // Combina as rotações
        Quaternion finalRotation = surfaceRotation * yRotation;

        Instantiate(prefab, position, finalRotation, transform);
    }

    [ContextMenu("Respaw Prefabs")]
    public void RespawnPrefabs()
    {
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        SpreadPrefabs();
    }

    void OnDrawGizmosSelected()
    {
        if (!drawSpawnPoints) return;

        Collider boxCollider = GetComponent<Collider>();
        if (boxCollider == null) return;

        Bounds bounds = boxCollider.bounds;
        Gizmos.color = spawnPointColor;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
