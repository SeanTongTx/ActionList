using SeanLib.Core;
using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class ActionController 
{
    [Serializable]
    public class ActionStatement
    {
        [InspectorPlus.OnValueChanged("OnNameChange")]
        public string name;
        public ActionList actions;
         
        void OnNameChange()
        {
            if (actions)
            {
                actions.gameObject.name = name;
            }
        }

    }
    public ActionPlayer PackPlayer;
    public List<ActionStatement> Statements;

    public bool Debug;
    public bool Playing()
    {
        return PackPlayer.state == ActionBase.ActionState.Play;
    }
    public bool PlayState(string StateName,Action onEnd=null, bool immdiately=false)
    {
        if (Debug)
        {
            StateDebug(StateName);
        }
       ActionStatement state= Statements.Find(e => e.name == StateName);
        if(state==null)return false;
        if (PackPlayer.SetCurrent(state.actions, onEnd, immdiately))
        {
            PackPlayer.Restart();
            return true;
        }
        return false;
    }

    public void Stop()
    {
        if (PackPlayer.state == ActionPlayer.ActionPlayerState.Play)
        {
            PackPlayer.Stop();
        }
    }

    public void StateDebug(string StateName)
    {
        DebugConsole.Info("动作", "PlayState", StateName, PackPlayer.name);
    }
}
