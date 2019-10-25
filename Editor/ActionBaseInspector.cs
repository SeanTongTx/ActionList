using EditorPlus;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
[CanEditMultipleObjects]
[CustomEditor(typeof(ActionBase), true)]
public class ActionBaseInspector : MonoBehaviourInspector
{
    private static GUIStyle style;

    public static GUIStyle ActionStyle
    {
        get
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.GetStyle("flow node 0")); 
                style.normal.textColor = Color.white;
                style.fontStyle = FontStyle.Bold;
                style.fixedHeight = 16;
                style.alignment = TextAnchor.UpperCenter;
                style.contentOffset = new Vector2(0, style.contentOffset.y-6);
            }
            return style;
        }
    }
    public override void OnInspectorGUI()
    {
        ActionBase action = target as ActionBase;

        GUI.backgroundColor = action.ActionColor;
        if (string.IsNullOrEmpty(action.ActionName))
        {
            action.ActionName = action.GetType().FullName;
        }
        GUILayout.Box(action.ActionName, ActionStyle, GUILayout.ExpandWidth(true));
        GUI.backgroundColor = Color.white;
        string verify = action.Verify();
        if (verify != string.Empty)
        {
            EditorGUILayout.HelpBox(verify, MessageType.Error);
        }
        if (OnGUIUtility.OpenClose("EditAction", target))
        {
            base.OnInspectorGUI();
        }
    }

}