using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
[CustomPropertyDrawer(typeof(ActionList))]
public class ActionListProperty : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect propRect = new Rect(position.x, position.y, position.width - 50, position.height);
        EditorGUI.BeginChangeCheck();

        EditorGUI.PropertyField(propRect, property, label);
        if (property.objectReferenceValue == null && GUI.Button(new Rect(position.x + propRect.width, position.y, 50, position.height), "Create"))
        {
            ActionList list = CreateActionList();
            list.gameObject.name = label.text;
            property.objectReferenceValue = list as Object;
        }
        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    ActionList CreateActionList()
    {
        return CreateActionObject.AddActionList();
    }
}
