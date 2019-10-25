using SeanLib.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAction : ActionBase
{
    public override Color ActionColor
    {
        get
        {
            return Color.green;
        }
    }
    [InspectorPlus.Orderable]
    public List<ActionBase> actions=new List<ActionBase>();
    public override void Play()
    {
        base.Play();
        foreach (ActionBase action in actions)
        {
            action.Stop();
        }
        this.End();
    }

    public override string Verify()
    {
        return actions.Exists(e => e == null) ? "null Action" : string.Empty;
    }
}
