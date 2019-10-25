using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheckAction : ActionPack
{
    public override Color ActionColor
    {
        get
        {
            return Color.blue * 0.8f;
        }
    }

    public ActionList IfTrue;
    public ActionList Else;

    public abstract bool Check();

    public override void Play()
    {
        if (this.Check())
        {
            this.PackActions = this.IfTrue;
        }
        else if(Else)
        {
            this.PackActions = Else;
        }
        base.Play();
    }
}