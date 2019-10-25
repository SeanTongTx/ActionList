using EditorPlus;
using SeanLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

using Object = UnityEngine.Object;
[CustomSeanLibEditor("ActionList/ListWindow")]
public class ActionListWindow : SeanLibEditor
{
    public static ActionList Current;

    private SerializedObject obj;
    private OnGUIUtility.FoldoutGroup AllList = new OnGUIUtility.FoldoutGroup("AllList");
    private OnGUIUtility.FoldoutGroup AllPack = new OnGUIUtility.FoldoutGroup("AllPack");
    private OnGUIUtility.FoldoutGroup AllPlayer = new OnGUIUtility.FoldoutGroup("AllPlayer");
    private OnGUIUtility.FoldoutGroup AllAction = new OnGUIUtility.FoldoutGroup("AllAction");
    private Vector2 v1;
    private Vector2 v2;
    private Vector2 v3;
    private Vector2 v4;



    public Dictionary<string, ReorderableList> RecorderLists = new Dictionary<string, ReorderableList>();
    List<int> deleteList = new List<int>();
    private ReorderableList currentList;
    public void SetActionlist(ActionList serializedObject)
    {
        Current = serializedObject;
        obj = new SerializedObject(Current);
        SerializedProperty property = obj.GetIterator();
        bool enterChildren = true;
        while (property.NextVisible(enterChildren))
        {
            if (property.propertyType == SerializedPropertyType.Generic && property.type == "vector")
            {
                ReorderableList list = new ReorderableList(obj, obj.FindProperty(property.propertyPath), true, true, true, true);
                list.drawElementCallback = DrawListElement;
                list.drawHeaderCallback = DrawHeader;
                list.drawFooterCallback = DrawFooter;
                list.onRemoveCallback = Remove;
                RecorderLists[property.name] = list;
            }
            enterChildren = false;
        }
    }

    private void DrawListElement(Rect rect, int index, bool isactive, bool isfocused)
    {
        SerializedProperty itemData = obj.FindProperty(currentList.serializedProperty.propertyPath).GetArrayElementAtIndex(index);
        rect.y += 2;
        rect.height = EditorGUIUtility.singleLineHeight;
        rect.width -= rect.height;
        EditorGUI.PropertyField(rect, itemData, GUIContent.none);
        if (GUI.Button(new Rect(rect.x + rect.width, rect.y, rect.height, rect.height), "X"))
        {
            deleteList.Add(index);
        }
    }

    private void DrawFooter(Rect rect)
    {
        float xMax = rect.xMax;
        float num = xMax - 8f;
        if (currentList.displayAdd)
        {
            num -= 25f;
        }
        if (currentList.displayRemove)
        {
            num -= 25f;
        }
        Rect r0 = new Rect(rect.x, rect.y, 25f, 13f);
        Rect p0 = new Rect(rect.x, rect.y - 4f, 25f, 13f);
        rect = new Rect(num, rect.y, xMax - num, rect.height);
        Rect rect2 = new Rect(num + 4f, rect.y - 3f, 25f, 13f);
        Rect rect3 = new Rect(xMax - 29f, rect.y - 3f, 25f, 13f);

        if (Event.current.type == EventType.Repaint)
        {
            ReorderableList.defaultBehaviours.footerBackground.Draw(rect, false, false, false, false);
            ReorderableList.defaultBehaviours.footerBackground.Draw(r0, false, false, false, false);
        }
        if (GUI.Button(p0, "c", ReorderableList.defaultBehaviours.preButton))
        {
            IList list = PropertyDrawerTools.GetPropertyInstance<IList>(currentList.serializedProperty);
            if (list != null)
            {
                list.Clear();
            }
        }
        if (currentList.displayAdd)
        {
            if (GUI.Button(
                rect2,
                (currentList.onAddDropdownCallback == null) ? ReorderableList.defaultBehaviours.iconToolbarPlus : ReorderableList.defaultBehaviours.iconToolbarPlusMore,
                ReorderableList.defaultBehaviours.preButton))
            {
                if (currentList.onAddDropdownCallback != null)
                {
                    currentList.onAddDropdownCallback(rect2, currentList);
                }
                else if (currentList.onAddCallback != null)
                {
                    currentList.onAddCallback(currentList);
                }
                else
                {
                    ReorderableList.defaultBehaviours.DoAddButton(currentList);
                }
                if (currentList.onChangedCallback != null)
                {
                    currentList.onChangedCallback(currentList);
                }
            }
        }
        if (currentList.displayRemove)
        {
            using (new EditorGUI.DisabledScope(currentList.index < 0 || currentList.index >= currentList.count || (currentList.onCanRemoveCallback != null && !currentList.onCanRemoveCallback(currentList))))
            {
                if (GUI.Button(rect3, ReorderableList.defaultBehaviours.iconToolbarMinus, ReorderableList.defaultBehaviours.preButton))
                {
                    Remove(currentList);
                    if (currentList.onChangedCallback != null)
                    {
                        currentList.onChangedCallback(currentList);
                    }
                }
            }
        }
    }

    private void Remove(ReorderableList list)
    {
        IList l = PropertyDrawerTools.GetPropertyInstance<IList>(currentList.serializedProperty);
        if (l != null)
        {
            l.RemoveAt(list.index);
        }
    }
    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, currentList.serializedProperty.name);
        var eventType = Event.current.type;
        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            if (rect.Contains(Event.current.mousePosition))
            {
                // Show a copy icon on the drag
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    if (DragAndDrop.objectReferences.Length > 0)
                    {
                        IList list = PropertyDrawerTools.GetPropertyInstance<IList>(currentList.serializedProperty);
                        if (list != null)
                        {
                            Type t = list.GetType().GetGenericArguments()[0];
                            foreach (Object reference in DragAndDrop.objectReferences)
                            {
                                if (reference is GameObject)
                                {
                                    if (t == typeof(GameObject))
                                    {
                                        list.Add(reference);
                                    }
                                    else
                                    {
                                        Component c = (reference as GameObject).GetComponent(t);
                                        if (c)
                                        {
                                            list.Add(c);
                                        }
                                    }
                                }
                                else if (reference.GetType() == t || reference.GetType().IsSubclassOf(t))
                                {
                                    list.Add(reference);
                                }
                            }
                        }
                    }
                }
                Event.current.Use();
            }
        }
    }
    public override void OnGUI()
    {
        base.OnGUI();
        if (Current != null)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Current ActionList",Current, typeof(ActionList), true);
            GUI.enabled = true;
            EditorGUI.BeginChangeCheck();
            obj.Update();
            SerializedProperty property = obj.GetIterator();
            bool enterChildren = true;
            while (property.NextVisible(enterChildren))
            {
                using (new EditorGUI.DisabledScope("m_Script" == property.propertyPath))
                {
                    if (property.propertyType == SerializedPropertyType.Generic && property.type == "vector")
                    {
                        InspectorPlus.Singleton singleAtt = PropertyDrawerTools.GetAttribute<InspectorPlus.Singleton>(property);
                        if (singleAtt != null)
                        {
                            IList listInstance = PropertyDrawerTools.GetPropertyInstance<IList>(property);
                            if (listInstance != null)
                            {
                                for (int i = 0; i < listInstance.Count; i++)
                                {
                                    var listElement = listInstance[i];
                                    for (int j = listInstance.Count - 1; j > i; j--)
                                    {
                                        var listElement1 = listInstance[j];
                                        if (listElement == listElement1)
                                        {
                                            listInstance.RemoveAt(j);
                                        }
                                    }
                                }
                            }
                        }
                        InspectorPlus.Orderable att = PropertyDrawerTools.GetAttribute<InspectorPlus.Orderable>(property);
                        if (att != null)
                        {
                            ReorderableList list = RecorderLists[property.name];
                            currentList = list;
                            bool show = EditorPrefs.GetBool(property.propertyPath);
                            show = EditorGUILayout.Foldout(show, property.name);
                            if (show)
                            {
                                
                                list.DoLayoutList();
                                foreach (int i in deleteList)
                                {
                                    property.DeleteArrayElementAtIndex(i);
                                }
                                deleteList.Clear();
                            }
                            EditorPrefs.SetBool(property.propertyPath, show);
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
                        }
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
                    }
                }
                enterChildren = false;
            }
            obj.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
        DebugAllList();
        DebugAllPack();
        DebugPlayer();
        DebugActions();
    }

    private void DebugActions()
    {
        if (AllAction.OnGui())
        {
            v4 = EditorGUILayout.BeginScrollView(v4);
            ActionBase[] apacksLists = GameObject.FindObjectsOfType<ActionBase>();
            foreach (ActionBase pack in apacksLists)
            {
                string error = pack.Verify();
                if (error != string.Empty)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(error, GUI.skin.GetStyle("ErrorLabel"));
                    EditorGUILayout.ObjectField(pack, typeof(ActionBase), true);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void DebugPlayer()
    {
        if (AllPlayer.OnGui())
        {
            v3 = EditorGUILayout.BeginScrollView(v3);
            ActionPlayer[] apacksLists = GameObject.FindObjectsOfType<ActionPlayer>();
            foreach (ActionPlayer pack in apacksLists)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(pack.state.GetEnumStr(), GUI.skin.GetStyle("ErrorLabel"));
                EditorGUILayout.ObjectField(pack, typeof(ActionPlayer), true);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void DebugAllPack()
    {
        if (AllPack.OnGui())
        {
            EditorGUI.indentLevel = 1;
            v2 = EditorGUILayout.BeginScrollView(v2);
            ActionPack[] apacksLists = GameObject.FindObjectsOfType<ActionPack>();
            foreach (ActionPack pack in apacksLists)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(pack.ActionName);
                EditorGUILayout.ObjectField(pack, typeof(ActionPack), true);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel = 0;
        }
    }

    void DebugAllList()
    {
        if (AllList.OnGui())
        {
            EditorGUI.indentLevel = 1;
            v1 = EditorGUILayout.BeginScrollView(v1);
            ActionList[] actionsLists = GameObject.FindObjectsOfType<ActionList>();
            foreach (ActionList actionList in actionsLists)
            {
                EditorGUILayout.BeginHorizontal();
                if (actionList.Actions.Exists(e => e == null))
                {
                    EditorGUILayout.LabelField("Action NULL", GUI.skin.GetStyle("ErrorLabel"));
                }
                else
                {
                    EditorGUILayout.LabelField("------");
                }
                EditorGUILayout.ObjectField(actionList, typeof(ActionList), true);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel = 0;
        }
    }

    /* 
      /// <summary>
      /// 拖入贴图数据
      /// </summary>
      void HandleDragAndDrop()
      {
          ISerializedObject serializedObject = Current as ISerializedObject;
          if (serializedObject == null)
          {
              return;
          }
          var eventType = Event.current.type;
          if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
          {
              Rect rect = new Rect(0, 0, Screen.width, Screen.height);
              if (rect.Contains(Event.current.mousePosition))
              {
                  // Show a copy icon on the drag
                  DragAndDrop.visualMode = DragAndDropVisualMode.Link;

                  if (eventType == EventType.DragPerform)
                  {
                      DragAndDrop.AcceptDrag();
                      if (DragAndDrop.objectReferences.Length > 0)
                      {
                          foreach (Object reference in DragAndDrop.objectReferences)
                          {
                              if (reference is GameObject)
                              {
                                  ActionList component = (reference as GameObject).GetComponent<ActionList>();
                                  if (component != null) Current = component;
                                  break;
                              }
                              else if (reference is ActionList)
                              {
                                  Current = reference as ActionList;
                              }
                          }
                      }
                  }
                  Event.current.Use();
              }
          }
      }*/
}
