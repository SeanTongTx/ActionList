using SeanLib.Core;
using System.Collections;



using UnityEngine;

public class ActionBase : MonoBehaviour
{
    [HideInInspector]
    public virtual Color ActionColor
    {
        get { return Color.white; }
    }
    public enum Timing
    {
        Custom,
        Awake,
        Start,
        Enable,
        EndFrame
    }
    public enum ActionState
    {
        Idle,
        Play,
        Stop,
        End
    }
    [Tooltip("执行时机")]
    public Timing timing;
    public string ActionName;
    [HideInInspector]
    public State State = new State(ActionState.Idle);
    [HideInInspector]
    public ActionPlayer CurrentPlayer;

    public void Awake()
    {
        if (timing == Timing.Awake)
        {
            this.Play();
        }
        else if (timing == Timing.EndFrame)
        {
            StartCoroutine(this.OnEndFrame());
        }
    }

    protected virtual IEnumerator OnEndFrame()
    {
        yield return new WaitForEndOfFrame();
        this.Play();
    }
    public void Start()
    {
        if (timing == Timing.Start)
        {
            this.Play();
        }
    }

    public void OnEnable()
    {
        if (timing == Timing.Enable)
        {
            this.Play();
        }
    }


    public ActionEnvironment Environment
    {
        get
        {
            if (CurrentPlayer)
            {
                return CurrentPlayer.Environment;
            }
            else
            {
                return null;
            }
        }
    }
    public virtual void Reset()
    {
        State.SetState(ActionState.Idle);
    }
    public virtual void Play()
    {
        DebugConsole.Info("动作", "执行", string.IsNullOrEmpty(this.ActionName) ? this.GetType().Name : ActionName);
        State.SetState(ActionState.Play);
    }

    public virtual void Stop()
    {
        State.SetState(ActionState.Stop);
    }

    public virtual void End()
    {
        State.SetState(ActionState.End);
        if (CurrentPlayer && CurrentPlayer.state == ActionPlayer.ActionPlayerState.Wait)
        {
            CurrentPlayer.Play();
        }
    }
    public virtual void Revert()
    {

    }
    public virtual string Verify()
    {
        return string.Empty;
    }

}
