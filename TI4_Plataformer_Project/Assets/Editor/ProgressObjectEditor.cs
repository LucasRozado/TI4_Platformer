using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Progress), true)]
public class ProgressObjectEditor : Editor
{
    Progress cod;

    private void OnEnable()
    {
        cod = (Progress)target;
    }

    public override void OnInspectorGUI()
    {
        if (cod.levelProgress != null)
        {
            EditorGUILayout.LabelField(cod.levelProgress.data.elementName[cod.intReference], GUI.skin.button);
        }        
        base.OnInspectorGUI();        
    }
}
