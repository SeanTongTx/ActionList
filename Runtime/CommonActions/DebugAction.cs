using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAction : EventAction
{
    public string Log = "DebugAction";

    public override void Play()
    {
        base.Play();
        Debug.Log(Log);
        this.End();
    }
}
