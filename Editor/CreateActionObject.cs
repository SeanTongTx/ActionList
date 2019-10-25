using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class CreateActionObject
{

    [MenuItem("GameObject/Create Other/Actions/Pack")]
    public static void AddActionPack()
    {
        ActionPack newpack = ActionPack.New();
        Set2Object(newpack.gameObject);
    }
    [MenuItem("GameObject/Create Other/Actions/Loop")]
    public static void AddLoop()
    {
        ActionPack newpack = LoopAction.NewObject();
        Set2Object(newpack.gameObject);
    }
    [MenuItem("GameObject/Create Other/Actions/ActionList")]
    public static ActionList AddActionList()
    {
        GameObject ActionList = new GameObject { name = "ActionList" };
        ActionList list = ActionList.AddComponent<ActionList>();
        if (Selection.activeGameObject != null)
        {
            ActionList.transform.SetParent(Selection.activeGameObject.transform, false);
        }
        return list;
    }
    private static void Set2Object(GameObject o)
    {
        if (Selection.activeGameObject != null)
        {
            o.transform.SetParent(Selection.activeGameObject.transform, false);
        }
        Selection.activeGameObject = o;
    }
}
