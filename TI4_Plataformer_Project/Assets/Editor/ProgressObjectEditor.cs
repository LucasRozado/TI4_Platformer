using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Progress), true)]
public class ProgressObjectEditor : Editor
{
    [SerializeField] int progress = 0;
    Progress cod;
    SerializedObject soCod;
    SerializedProperty codProgress;

    private void OnEnable()
    {
        cod = (Progress)target;
        soCod = new SerializedObject(cod);
        codProgress = soCod.FindProperty("intReference");
    }

    public override void OnInspectorGUI()
    {
        if (cod.levelProgress != null)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("Choose Progress");
            if (GUILayout.Button(cod.levelProgress.data.elementName[codProgress.intValue]))
            {
                GenericMenu menu = new GenericMenu();
                for (int i = 0; i < cod.levelProgress.data.elementName.Length; i++)
                {
                    AddStringToMenu(menu, i);
                }
                menu.ShowAsContext();
            }
            GUILayout.EndHorizontal();
        }              
               
        base.OnInspectorGUI();        
    }

    private void AddStringToMenu(GenericMenu menu, int i)
    {
        menu.AddItem(new GUIContent(cod.levelProgress.data.elementName[i]), i.Equals(cod.intReference), OnDropSelected, i);
    }

    public void OnDropSelected(object obj)
    {
        Undo.SetCurrentGroupName("Add Progress Element");
        int i = Undo.GetCurrentGroup();
        soCod.Update();

        progress = (int)obj;
        codProgress.intValue = progress;

        soCod.ApplyModifiedProperties();
        Undo.CollapseUndoOperations(i);
    }
}
