

using EditorPlus;
using UnityEditor;

using UnityEngine;
[CustomPropertyDrawer(typeof(ActionBase),true)]
public class ActionBasePropertyEditor : DefaultPropertyDrawer
{
    protected override void OnDraw(Rect position, SerializedProperty property, GUIContent label)
    {
        ActionBase instance = PropertyDrawerTools.GetPropertyInstance<ActionBase>(property);
        if (instance)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (label.text != string.Empty)
            {
                Rect r1 = OnGUIUtility.Layout.Divide.FirstRect(position, 3);
                EditorGUI.PrefixLabel(r1, label);
                Rect r2 = OnGUIUtility.Layout.Divide.NextRect(r1);
                GUI.backgroundColor = instance.ActionColor;
                EditorGUI.LabelField(r2, instance.ActionName, ActionBaseInspector.ActionStyle);
                GUI.backgroundColor = Color.white;
                Rect r3 = OnGUIUtility.Layout.Divide.NextRect(r2);
                property.objectReferenceValue = EditorGUI.ObjectField(r3, instance, typeof(ActionBase), true);
            }
            else
            {
                Rect r2;
                Rect r1 = OnGUIUtility.Layout.Divide.Golden(position, out r2);
                GUI.backgroundColor = instance.ActionColor;
                EditorGUI.LabelField(r1, instance.ActionName, ActionBaseInspector.ActionStyle);
                GUI.backgroundColor = Color.white;
                property.objectReferenceValue = EditorGUI.ObjectField(r2, instance, typeof(ActionBase), true);
            }
            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

}