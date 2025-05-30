using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelProgress))]
public class LevelProgressEditor : Editor
{
    LevelProgress cod;
    SerializedObject soCod;
    SerializedProperty progressName;
    SerializedProperty progressBool;
    private void OnEnable()
    {
        cod = (LevelProgress)target;
        soCod = new SerializedObject(cod);
        progressName = soCod.FindProperty("elementName");
        progressBool = soCod.FindProperty("levelProgress");
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Add Element"))
        {
            AddElement();
        }
        if (GUILayout.Button("Remove Element"))
        {
            RemoveElement();
        }
        for (int i = 0; i < progressName.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(progressName.GetArrayElementAtIndex(i));  
            EditorGUILayout.EndHorizontal();
        }

        base.OnInspectorGUI();
    }
    public void AddElement()
    {
        soCod.Update();
        progressName.InsertArrayElementAtIndex(progressName.arraySize);
        progressBool.InsertArrayElementAtIndex(progressBool.arraySize);
        soCod.ApplyModifiedProperties();        
    }
    public void RemoveElement()
    {
        soCod.Update();
        progressName.DeleteArrayElementAtIndex(progressName.arraySize - 1);
        progressBool.DeleteArrayElementAtIndex(progressBool.arraySize - 1);
        soCod.ApplyModifiedProperties();
    }
}
