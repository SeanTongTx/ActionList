using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class PlayRandomAnimation : PlayAnimation
{
    public override void Play()
    {
        base.Play();
        if (Controls.Count > 0)
        {
            AnimControlValue control = Controls[UnityEngine.Random.Range(0, Controls.Count)];
            if (control.animator)
            {
                if (!string.IsNullOrEmpty(control.StateName))
                {
                    control.animator.Play(control.StateName);
                }
                control.SetValue(control.animator);
            }
            if (animTime == 0)
                this.End();
            else
            {
                this.Invoke("End", animTime);
            }
        }
        else
        {
            this.End();
        }
        foreach (AnimControlValue control in Controls)
        {

        }

    }
#if UNITY_EDITOR
    public PlayAnimation CopyFrom;
    [ContextMenu("SetData")]
    public void SetData()
    {
        this.Controls.Clear();
        foreach (AnimControlValue value in CopyFrom.Controls)
        {
            this.Controls.Add(value);
        }
        this.animTime = CopyFrom.animTime;
        this.ActionName = CopyFrom.ActionName;
        this.timing = CopyFrom.timing;
    }
#endif
}
