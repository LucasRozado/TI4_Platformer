using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabPlacer : EditorWindow
{
    [MenuItem("Tools/PrefabPlacer")]
    public static void OpenPrefabPlacer() => GetWindow<PrefabPlacer>();

    public float radius = 5f;
    public int spawnCount = 8;
    public LayerMask layerToHit;
    [Header("Prefabs")]
    public GameObject[] spawnPrefab = null;
    public bool allignToNormal;
    public float maxScale = 1f;
    public float minScale = 1f;

    SerializedObject so;
    SerializedProperty propRadius;
    SerializedProperty propSpawnCount;
    SerializedProperty propLayerMask;
    SerializedProperty propSpawnPrefab;
    SerializedProperty propAllignToNormal;
    SerializedProperty propMaxScale;
    SerializedProperty propMinScale;

    Vector2[] randomPoints;

    private void OnEnable()
    {
        so = new SerializedObject(this);

        propRadius = so.FindProperty("radius");
        propSpawnCount = so.FindProperty("spawnCount");
        propLayerMask = so.FindProperty("layerToHit");
        propSpawnPrefab = so.FindProperty("spawnPrefab");
        propAllignToNormal = so.FindProperty("allignToNormal");
        propMaxScale = so.FindProperty("maxScale");
        propMinScale = so.FindProperty("minScale");

        GenerateRandomPoints();
        
        SceneView.duringSceneGui += DuringSceneGUI;
    }
    private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

    public void GenerateRandomPoints()
    {
        randomPoints = new Vector2[spawnCount];
        for (int i = 0; i < spawnCount; ++i)
        {
            randomPoints[i] = Random.insideUnitCircle;
        }
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propRadius);
        propRadius.floatValue = Mathf.Max(1f, propRadius.floatValue);
        EditorGUILayout.PropertyField(propSpawnCount);
        propSpawnCount.intValue = (int)Mathf.Max(1, propSpawnCount.intValue);
        EditorGUILayout.PropertyField(propLayerMask);
        EditorGUILayout.PropertyField(propSpawnPrefab);
        EditorGUILayout.PropertyField(propAllignToNormal);
        EditorGUILayout.PropertyField(propMaxScale);
        propMaxScale.floatValue = Mathf.Max(0.1f, propMaxScale.floatValue);
        EditorGUILayout.PropertyField(propMinScale);
        propMinScale.floatValue = Mathf.Max(0.1f, propMinScale.floatValue);

        if (so.ApplyModifiedProperties())
        {
            GenerateRandomPoints();
            SceneView.RepaintAll();
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GUI.FocusControl(null);
            Repaint();
        }

    }

    void DrawSphere(Vector3 pos)
    {
        Handles.SphereHandleCap(-1, pos, Quaternion.identity, 0.3f, EventType.Repaint);
    }

    void TrySpawnPrefab(List<RaycastHit> hitPts)
    {
        if (spawnPrefab == null)
        {
            return;
        }

        foreach (RaycastHit hit in hitPts)
        {
            Quaternion rot;
            if (propAllignToNormal.boolValue)
            {
                rot = Quaternion.LookRotation(hit.normal);
            }
            else
            {
                rot = Quaternion.LookRotation(Vector3.up);
            }

            int randomPrefab = Random.Range(0, spawnPrefab.Length);
            if (spawnPrefab[randomPrefab] == null)
            {
                return;
            }

            rot *= Quaternion.Euler(90, 0, 0);
            rot *= Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(spawnPrefab[randomPrefab]);
            Undo.RegisterCreatedObjectUndo(spawned, "Spawned Object");
            spawned.transform.position = hit.point;
            spawned.transform.rotation = rot;
            spawned.transform.parent = hit.collider.transform;

            float scaleX = Random.Range(minScale, maxScale);
            float scaleY = Random.Range(minScale, maxScale);
            float scaleZ = Random.Range(minScale, maxScale);
            Vector3 scale = new Vector3(scaleX, scaleY, scaleZ);
            spawned.transform.localScale = scale;

            spawned.isStatic = true;
        }
    }

    public void DuringSceneGUI(SceneView sceneView)
    {
        Transform cameraTf = sceneView.camera.transform;

        if (Event.current.type == EventType.MouseMove)
        {
            sceneView.Repaint();
        }

        bool isHoldingAlt = (Event.current.modifiers & EventModifiers.Alt) != 0;

        if (Event.current.type == EventType.ScrollWheel && isHoldingAlt)
        {
            float scrollDir = Mathf.Sign(Event.current.delta.y);
            so.Update();
            propRadius.floatValue *= 1 + scrollDir * 0.05f;
            so.ApplyModifiedPropertiesWithoutUndo();
            Repaint();
            Event.current.Use();
        }

        

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, layerToHit, QueryTriggerInteraction.Ignore))
        {
            Vector3 hitNormal = hit.normal;
            Vector3 hitTangent = Vector3.Cross(hitNormal, cameraTf.up).normalized;
            Vector3 hitBitangent = Vector3.Cross(hitNormal, hitTangent).normalized;

            List<RaycastHit> hitPts = new List<RaycastHit>();

            foreach (Vector2 p in randomPoints)
            {
                Vector3 rayOrigin = hit.point + (hitTangent * p.x + hitBitangent * p.y) * radius;
                rayOrigin += hitNormal * 2f;
                Vector3 rayDirection = -hitNormal;
                Ray subray = new Ray(rayOrigin, rayDirection);

                if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit pointHit, 5f, layerToHit, QueryTriggerInteraction.Ignore))
                {
                    hitPts.Add(pointHit);
                    DrawSphere(pointHit.point);
                    Handles.DrawAAPolyLine(pointHit.point, pointHit.point + pointHit.normal);
                }
            }

            Handles.color = Color.black;
            Handles.DrawWireDisc(hit.point, hit.normal, radius);
            Handles.color = Color.white;

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && isHoldingAlt)
            {
                TrySpawnPrefab(hitPts);
                GenerateRandomPoints();
            }
        }
    }

}
