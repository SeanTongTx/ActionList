

using EditorPlus;
using UnityEditor;

using UnityEngine;

[DisallowMultipleComponent]
[CustomEditor(typeof(ActionList), true)]
public class ActionListEditor : MonoBehaviourInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ActionList list = target as ActionList;
        if (list.Actions.Exists(e => e == null))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("Action Null", MessageType.Error);
            if (GUILayout.Button("Clear"))
            {
                list.Actions.RemoveAll(e => e == null);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("ShowActionWindow"))
        {
            SeanLibManager window = SeanLibManager.GetWindow<SeanLibManager>();
            SeanLibIndexItem libIndexItem = window.SeachIndex("ActionList/ListWindow");
            if (libIndexItem != null && libIndexItem.editor != null)
            {
                window.SelectIndex(libIndexItem.id);
                (libIndexItem.editor as ActionListWindow).SetActionlist(list);
            }
        }
    }
}