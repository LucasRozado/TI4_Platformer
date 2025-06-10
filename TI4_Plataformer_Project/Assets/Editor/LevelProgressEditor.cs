using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelProgress))]
public class LevelProgressEditor : Editor
{
    LevelProgress obj;
    SerializedObject so;
    SerializedProperty progressName;
    SerializedProperty levelProgress;
    SerializedProperty fileBaseName;
    private void OnEnable()
    {
        obj = (LevelProgress)target;
        so = new SerializedObject(obj);
        progressName = so.FindProperty("progressName");
        levelProgress = so.FindProperty("levelProgress");
        fileBaseName = so.FindProperty("fileBaseName");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
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
            EditorGUILayout.PropertyField(levelProgress.GetArrayElementAtIndex(i));
            EditorGUILayout.PropertyField(progressName.GetArrayElementAtIndex(i));  
            EditorGUILayout.EndHorizontal();
        }
    }
    public void AddElement()
    {
        so.Update();
        progressName.InsertArrayElementAtIndex(progressName.arraySize);
        levelProgress.InsertArrayElementAtIndex(levelProgress.arraySize);
        so.ApplyModifiedProperties();        
    }
    public void RemoveElement()
    {
        so.Update();
        progressName.DeleteArrayElementAtIndex(progressName.arraySize - 1);
        levelProgress.DeleteArrayElementAtIndex(levelProgress.arraySize - 1);
        so.ApplyModifiedProperties();
    }
}
