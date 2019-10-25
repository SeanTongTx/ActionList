using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentActivation : ActionBase
{
    public MonoBehaviour Component;

    public bool enable;
    public override void Play()
    {
        base.Play();
        Component.enabled = enable;
        this.End();
    }

    public override string Verify()
    {
        if (Component==null)
        {
            return "NUll Component";
        }
        else
        {
            return string.Empty;
        }
    }
}
