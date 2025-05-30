using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelProgress))]
public class LevelProgressEditor : Editor
{
    LevelProgress cod;
    SerializedObject soCod;
    SerializedProperty progressName;
    SerializedProperty progressBool;
    SerializedProperty codData;
    private void OnEnable()
    {
        cod = (LevelProgress)target;
        soCod = new SerializedObject(cod);
        codData = soCod.FindProperty("data");
        progressName = codData.FindPropertyRelative("elementName");
        progressBool = codData.FindPropertyRelative("levelProgress");
    }
    public override void OnInspectorGUI()
    {
        soCod.Update();

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
            EditorGUILayout.PropertyField(progressBool.GetArrayElementAtIndex(i),new GUIContent(""));  
            EditorGUILayout.PropertyField(progressName.GetArrayElementAtIndex(i));  
            EditorGUILayout.EndHorizontal();
        }
        soCod.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
    public void AddElement()
    {
        Undo.SetCurrentGroupName("Add Progress Element");
        int i = Undo.GetCurrentGroup();
        soCod.Update();
        progressName.InsertArrayElementAtIndex(progressName.arraySize);
        progressBool.InsertArrayElementAtIndex(progressBool.arraySize);
        soCod.ApplyModifiedProperties();
        Undo.CollapseUndoOperations(i);
    }
    public void RemoveElement()
    {
        Undo.SetCurrentGroupName("Remove Progress Element");
        int i = Undo.GetCurrentGroup();
        soCod.Update();
        progressName.DeleteArrayElementAtIndex(progressName.arraySize - 1);
        progressBool.DeleteArrayElementAtIndex(progressBool.arraySize - 1);
        soCod.ApplyModifiedProperties();
        Undo.CollapseUndoOperations(i);
    }
}
