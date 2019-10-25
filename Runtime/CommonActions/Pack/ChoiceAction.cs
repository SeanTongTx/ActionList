using SeanLib.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceAction : ActionPack
{
    public List<ActionList> options = new List<ActionList>();

    public override void Play()
    {
        DebugConsole.Info("动作", "执行", string.IsNullOrEmpty(this.ActionName) ? this.GetType().Name : ActionName);
        State.SetState(ActionState.Play);
    }

    public void Choose(int index)
    {
        if (options.Count > index)
        {
            this.PackActions = options[index];
            base.Play();
        }
    }

}
