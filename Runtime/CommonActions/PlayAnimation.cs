using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class PlayAnimation : ActionBase
{
    [Serializable]
    public class AnimControlValue
    {
        public Animator animator;
        public string key;
        [Tooltip("强制设置动画状态")]
        public string StateName;
        public bool b;

        public float f = 0f;

        public void SetValue(Animator animator)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (this.f != 0f)
            {
                animator.SetFloat(this.key, this.f);
            }
            else
            {
                animator.SetBool(this.key, this.b);
            }
        }

        public void Play()
        {
            if (animator)
            {
                if (!string.IsNullOrEmpty(StateName))
                {
                    animator.Play(StateName);
                }
                else
                {
                    SetValue(animator);
                }
            }
        }
    }

    public List<AnimControlValue> Controls = new List<AnimControlValue>();
    public float animTime;
    public override void Play()
    {
        base.Play();
        foreach (AnimControlValue control in Controls)
        {
            control.Play();
        }
        if (animTime == 0)
            this.End();
        else
        {
            this.Invoke("End", animTime);
        }

    }

    public override string Verify()
    {
        if (Controls.Find(e => e == null || e.animator == null) != null)
        {
            return "列表中包含null对象";
        }
        return string.Empty;
    }
}
