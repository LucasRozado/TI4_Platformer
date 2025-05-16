using UnityEngine;
using UnityEditor;
using System;
using Unity.VisualScripting;
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
            NumberCollectables();
        }
    }

    public void NumberCollectables()
    {
        Undo.SetCurrentGroupName("Set Collectables");
        int i = Undo.GetCurrentGroup();
        int count = 0;
        foreach (Collectable collectable in cod.levelCollectables)
        {
            Undo.RecordObject(collectable, "Collectable: " + count);
            collectable.number = count;
            collectable.gameObject.name = "Collectable: " + count.ToString();
            count++;
        }
        Undo.CollapseUndoOperations(i);
    }
}
