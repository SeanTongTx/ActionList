using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : ActionBase
{
    public ParticleSystem particle;
    
    public bool Wait2End;
    public override void Play()
    {
        base.Play();
        particle.Play();
        if (Wait2End && !particle.main.loop)
        {
            this.Invoke("End", particle.main.duration);
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