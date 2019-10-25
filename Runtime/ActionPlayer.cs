using SeanLib.Core;
using System;



using UnityEngine;

/// <summary>
/// 递归的实现 堆栈容易溢出
/// </summary>
public class ActionPlayer : MonoBehaviour
{
    public enum ActionPlayerState
    {
        Idle,
        Play,
        Pause,
        Wait
    }
    public State state = new State(ActionPlayerState.Idle);

    [Tooltip("字典缓存,用来方便动作间数据交换\r\n每次执行结束就清理")]
    public ActionEnvironment Environment = new ActionEnvironment();
    public Action OnEnd;
    public ActionList Current;
    public void Restart()
    {
        this.Reset();
        this.Play();
    }

    public ActionBase PlayingAction;
    public void Play()
    {
        try
        {
            if (state == ActionPlayerState.Pause)
            {
                return;
            }
            if (Current != null)
            {
                while ((this.PlayingAction = this.Current.GetNextAction()) != null)
                {
                    if (PlayingAction.CurrentPlayer != null && PlayingAction.CurrentPlayer != this && PlayingAction.State == ActionBase.ActionState.End)
                    {
                        DebugConsole.Warning("Action", "Play", "同一个动作对象在多个player中同步执行" + PlayingAction.CurrentPlayer.name + ":" + this.name);
                    }
                    state.SetState(ActionPlayerState.Play);
                    PlayingAction.CurrentPlayer = this;
                    PlayingAction.Play();
                    if (PlayingAction.State != ActionBase.ActionState.End)
                    {
                        state.SetState(ActionPlayerState.Wait);
                        return;
                    }
                }
            }
            state.SetState(ActionPlayerState.Idle);
            Environment.Clear();
            if (OnEnd != null)
            {
                OnEnd();
            }

        }
        catch (Exception e)
        {
            DebugConsole.Error("Action", "Play", e.StackTrace);
        }
    }

    /// <summary>
    /// 当前执行动作后暂停
    /// </summary>
    public void Pause()
    {
        if (state == ActionPlayerState.Play)
        {
            state.SetState(ActionPlayerState.Pause);
        }
    }
    /// <summary>
    /// 执行完当前动作以后停止
    /// </summary>
    public void Break()
    {
        if (state == ActionPlayerState.Play && PlayingAction)
        {
            //    PlayingAction.onEnd = stop;
        }
    }
    public void Continue()
    {
        if (state == ActionPlayerState.Pause)
        {
            Play();
        }
    }

    public bool SetCurrent(ActionList current, Action onEnd = null, bool immdiately = false)
    {
        if (!immdiately && !state.CheckState(ActionPlayerState.Idle))
        {
            DebugConsole.Warning("ActionPack", "设置执行列表", state.Current.ToString(), "失败");
            return false;
        }
        if (immdiately)
        {
            this.Stop();
        }
        OnEnd = onEnd;
        Current = current;
        return true;
    }
    public void Reset()
    {
        if (Current)
        {
            Current.index = 0;
            state.SetState(ActionPlayerState.Idle);
            foreach (ActionBase action in Current.Actions)
            {
                action.Reset();
            }
        }
    }

    public void Stop()
    {
        if (Current && state != ActionPlayerState.Idle)
        {
            foreach (ActionBase action in Current.Actions)
            {
                action.Stop();
            }
            Reset();
        }
    }
}
