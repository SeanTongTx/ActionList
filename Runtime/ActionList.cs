using SeanLib.Core;
using System.Collections.Generic;



using UnityEngine;
using UnityEngine.Events;

public class ActionList : MonoBehaviour
{
    [SerializeField]
    [InspectorPlus.Singleton]
    [InspectorPlus.Orderable]
  /*  [Title("动作列表")]
    [ListDrawerSettings(DraggableItems = true,ShowIndexLabels = true,ShowItemCount = true)]*/
    private List<ActionBase> actions = new List<ActionBase>();

    public List<ActionBase> Actions
    {
        get
        {
            return actions;
        }
        set
        {
            actions = value;
        }
    }
    [HideInInspector]
    public int index = 0;
    public ActionBase GetNextAction()
    {
        if (index >= actions.Count)
        {
            return null;
        }
        else
        {
            ActionBase action = null;
            do
            {
                action = actions[index++];
            }
            while (action == null && index < actions.Count);
            return action;
        }
    }

    public virtual void Add(ActionBase action)
    {
        actions.Add(action);
    }
    public virtual ActionBase Find(string ActionName)
    {
        return actions.Find(e => e.ActionName == ActionName);
    }
}