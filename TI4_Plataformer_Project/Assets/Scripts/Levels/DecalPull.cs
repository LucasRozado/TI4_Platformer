using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalPull : MonoBehaviour
{
    [Header("Decal Settings")]
    public List<Texture2D> decalTextures;
    public Material decalMaterialTemplate;
    public float spawnChance = 0.7f;
    [Range(0f, 360f)] public float maxYRotationVariation = 30f;
    public bool turnStatic = true;

    [Header("Scale Randomization")]
    public bool randomizeScale = false;
    [MinMaxRange(0.1f, 3f)] public Vector2 scaleRangeX = new Vector2(0.8f, 1.2f);
    [MinMaxRange(0.1f, 3f)] public Vector2 scaleRangeY = new Vector2(0.8f, 1.2f);
    [MinMaxRange(0.1f, 3f)] public Vector2 scaleRangeZ = new Vector2(0.8f, 1.2f);
    public bool uniformScaling = true;

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

    [Header("Decal Projector Settings")]
    public float decalOffset = 0.4f; // Distância acima da superfície
    public float decalSize = 2f; // Tamanho padrão do decal projector

    [Header("Debug")]
    public bool drawDebugRays = true;
    public bool drawSpawnPoints = true;
    public Color spawnPointColor = Color.cyan;

    void Start()
    {
        if (decalTextures == null || decalTextures.Count == 0)
        {
            Debug.LogWarning("Nenhuma textura de decal atribuída.");
            return;
        }

        if (decalMaterialTemplate == null)
        {
            Debug.LogWarning("Nenhum material template atribuído para os decals.");
            return;
        }

        SpreadDecals();
    }

    void SpreadDecals()
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
                        SpawnDecalAtPoint(hit.point, hit.normal);
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

    void SpawnDecalAtPoint(Vector3 position, Vector3 surfaceNormal)
    {
        if (decalTextures.Count == 0) return;

        // Seleciona uma textura aleatória
        Texture2D selectedTexture = decalTextures[Random.Range(0, decalTextures.Count)];

        // Cria o GameObject do Decal
        GameObject decal = new GameObject($"Decal_{selectedTexture.name}");
        decal.transform.position = position + (surfaceNormal * decalOffset);
        decal.transform.SetParent(transform);

        // Adiciona e configura o DecalProjector
        DecalProjector projector = decal.AddComponent<DecalProjector>();

        // Cria uma NOVA instância do material para cada decal
        Material decalMaterial = new Material(decalMaterialTemplate);

        // Configuração robusta das propriedades do material
        if (decalMaterial.HasProperty("_BaseColorMap"))
        {
            decalMaterial.SetTexture("_BaseColorMap", selectedTexture);
        }
        else if (decalMaterial.HasProperty("_MainTex"))
        {
            decalMaterial.SetTexture("_MainTex", selectedTexture);
        }

        // Aplica o material ao projector
        projector.material = decalMaterial;

        // Configurações básicas do projector
        projector.size = new Vector3(decalSize, decalSize, decalSize);
        projector.pivot = Vector3.zero;
        projector.fadeFactor = 1f;

        // Orientação do decal (Z apontando para -Y)
        Quaternion baseRotation = Quaternion.Euler(90f, 0f, 0f);

        if (alignToSurface)
        {
            Quaternion surfaceAlignment = Quaternion.FromToRotation(Vector3.down, surfaceNormal);
            decal.transform.rotation = surfaceAlignment * baseRotation;
        }
        else
        {
            decal.transform.rotation = baseRotation;
        }

        // Rotação aleatória no eixo Y
        decal.transform.Rotate(0f, Random.Range(-maxYRotationVariation, maxYRotationVariation), 0f, Space.Self);

        // Randomização de escala
        if (randomizeScale)
        {
            Vector3 scale = uniformScaling ?
                Vector3.one * Random.Range(scaleRangeX.x, scaleRangeX.y) :
                new Vector3(
                    Random.Range(scaleRangeX.x, scaleRangeX.y),
                    Random.Range(scaleRangeY.x, scaleRangeY.y),
                    Random.Range(scaleRangeZ.x, scaleRangeZ.y));

            projector.size = Vector3.Scale(new Vector3(decalSize, decalSize, decalSize), scale);
        }

        if (turnStatic)
        {
            decal.isStatic = true;
        }
    }

    [ContextMenu("Respawn Decals")]
    public void RespawnDecals()
    {
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        SpreadDecals();
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