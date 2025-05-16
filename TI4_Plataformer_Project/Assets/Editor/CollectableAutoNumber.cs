using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CollectablesList))]
public class CollectableAutoNumber : Editor
{
    CollectablesList cod;
    Collectable[] list;
    private void OnEnable()
    {
        cod = (CollectablesList)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Listar Collectables"))
        {
            cod.NumberCollectables();
        }
    }
}
