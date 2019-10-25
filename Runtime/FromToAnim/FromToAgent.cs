using System;

using UnityEngine;
using System.Collections;
using SeanLib.Core;

[Serializable]
public class FromToAgent
{
    public enum AnimType
    {
        Once,
        Loop,
        Pinpong,
        OncePinOncePong
    }

    public AnimType type = AnimType.Once;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField, InspectorPlus.SetProperty("from")]
    private float _from;
    public float from
    {
        get
        {
            return this.forward ? this._from : this._to;
        }
        set
        {
            _from = value;
        }
    }

    [SerializeField]
    [InspectorPlus.SetProperty("to")]
    private float _to = 1;

    public float to
    {
        get
        {
            return this.forward ? this._to : this._from;
        }
        set
        {
            _to = value;
        }
    }
    [Tooltip("动画时间")]
    public float secound = 1f;
    [Tooltip("当前进度")]
    public float Current = 0;

    public float Delay;

    public bool forward = true;
    public Action OnComplete;

    public Action OnUpdate;
    public Action OnStart;

    private float TimeRecord;
    private float c;
    public void Reset()
    {
        Current = from;
        TimeRecord = 0;
    }

    public void Start()
    {
        FromTo fromto = Instance.GetInstance<FromTo>();
        if (fromto)
        {
            fromto.RegistAgent(this);
            if (OnStart != null)
            {
                OnStart();
            }
        }
    }

    public void Stop()
    {
        FromTo fromto = Instance.GetInstance<FromTo>();
        if (fromto)
        {
            fromto.UnRegishAgent(this);
        }
    }
    public bool Update()
    {
        if (secound <= 0)
        {
            Current = to;
        }
        else
        {
            TimeRecord += Time.deltaTime;
            c = curve.Evaluate(TimeRecord / secound);
            Current = Mathf.LerpUnclamped(from, to, c);
        }
        if (OnUpdate != null)
        {
            OnUpdate();
        }
        if (TimeRecord >= secound)
        {
            switch (type)
            {
                case AnimType.Once:
                    Current = to;
                    return true;
                case AnimType.Loop:
                    Reset();
                    break;
                case AnimType.Pinpong:
                    forward = !forward;
                    Reset();
                    break;
                case AnimType.OncePinOncePong:
                    this.Reset();
                    forward = !forward;
                    return true;
            }
        }
        return false;
    }
}
