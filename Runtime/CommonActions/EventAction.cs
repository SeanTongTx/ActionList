using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAction : ActionBase
{
    public UnityEvent OnPlay;
    public override void Play()
    {
        base.Play();
        OnPlay.Invoke();
        this.End();
    }
}
