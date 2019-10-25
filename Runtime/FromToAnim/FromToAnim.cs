
using UnityEngine;

public abstract class FromToAnim : ActionBase
{
    public FromToAgent AnimController = new FromToAgent();

    public bool Wait2End;
    public new void Awake()
    {
        base.Awake();
        AnimController.OnUpdate = ApplyTrans;
    }

    public void OnDestroy()
    {
        AnimController.Stop();
    }

    public override Color ActionColor
    {
        get { return new Color(171f / 255f, 248f/255f,1,1); }
    }

    public abstract void ApplyTrans();
    public override void Play()
    {
        if (AnimController.Delay> 0)
        {
            this.Invoke("RealPlay", AnimController.Delay);
        }
        else
        {
            RealPlay();
        }
    }

    private void RealPlay()
    {
        base.Play();
       
        if (AnimController.secound <= 0)
        {
            AnimController.Current = AnimController.to;
            this.ApplyTrans();
            this.End();
        }
        else
        {
            AnimController.Start();
            if (Wait2End)
            {
                // AnimController.OnComplete = End;
                this.Invoke("End", AnimController.secound);
            }
            else
            {
                this.End();
            }
        }
    }
    public override void Stop()
    {
        base.Stop();
        this.CancelInvoke("End");
        AnimController.Stop();
    }
}
