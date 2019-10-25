using SeanLib.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAction : ActionPack
{
    public int LoopTime;

    public int i;

    public virtual int CheckLoopTime()
    {
        return LoopTime;
    }
    public override void Reset()
    {
        base.Reset();
        this.i = 0;
        LoopTime = CheckLoopTime();
    }

    public override void Play()
    {
        if (i >= LoopTime)
        {
            this.End();
        }
        while (this.i < LoopTime)
        {
            base.Play();
            this.i++;
            if (PackPlayer.state != ActionPlayer.ActionPlayerState.Idle)
            {
                //没有完全结束
                this.State.EnableState(ActionBase.ActionState.Stop);
                return;
            }
        }
    }

    public override void End()
    {
        if (this.i >= LoopTime)
        {
            base.End();
        }
        else
        {
            if (this.State.ContainState(ActionBase.ActionState.Stop))
            {
                this.State.DisableState(ActionBase.ActionState.Stop);
                this.Play();
            }
            else
            {
                DebugConsole.Warning("LoopAction", "End", "PlayNextError");
            }
        }
    }
    public static LoopAction NewObject()
    {
        GameObject pack = new GameObject { name = "Loop" };
        LoopAction actionpack = pack.AddComponent<LoopAction>();
        ActionPlayer player = pack.AddComponent<ActionPlayer>();
        ActionList list = pack.AddComponent<ActionList>();
        actionpack.PackPlayer = player;
        actionpack.PackActions = list;
        actionpack.name = "循环";
        return actionpack;
    }
}