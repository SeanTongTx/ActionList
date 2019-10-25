using UnityEngine;
using System.Collections;

public class Delay : ActionBase
{
    public float Secound = 2f;
    public override void Play()
    {
        base.Play();
        if (Secound > 0)
        {
            this.Invoke("End", Secound);
        }
        else
        {
            this.End();
        }
    }

    public override void Stop()
    {
        base.Stop();
        this.CancelInvoke("End");
    }
}