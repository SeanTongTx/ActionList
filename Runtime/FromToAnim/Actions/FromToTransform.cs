
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using SeanLib.Core;

public class FromToTransform : FromToAnim
{
    public enum AnimType
    {
        postion,
        scale,
        rotation,
        world,
        /// <summary>
        /// 从当前位置开始
        /// </summary>
        currentTransform,
        /// <summary>
        /// 偏移
        /// </summary>
        Offset,
        size,
    }
    public State animType = new State(AnimType.postion);

    [Space]
    [Header("变换")]
    public Vector3 rotFrom;
    public Vector3 rotTo;

    public Vector3 posFrom;
    public Vector3 posTo;

    public Vector3 scaleFrom;
    public Vector3 scaleTo;
    [Header("只对RectTransform生效")]
    public Vector2 sizeFrom;
    public Vector2 sizeTo;


    [SerializeField, InspectorPlus.SetProperty("TargetTransform")]
    private Transform targetTransform;
    private RectTransform targetRectTransform;
    public Transform TargetTransform
    {
        get
        {
            if (this.targetTransform == null)
            {
                this.targetTransform = this.transform;
            }
            if (targetRectTransform == null)
            {
                targetRectTransform = targetTransform as RectTransform;
            }
            return targetTransform;
        }
        set
        {
            targetTransform = value;
        }
    }
    [ContextMenu("ApplyCurrent2From")]
    public void ApplyCurrent2From()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Current2From");
#endif
        TargetTransform.localEulerAngles = rotFrom;
        TargetTransform.localPosition = posFrom;
        TargetTransform.localScale = scaleFrom;
        if (targetRectTransform)
        {
            targetRectTransform.sizeDelta = sizeFrom;
        }
    }
    [ContextMenu("ApplyCurrent2To")]
    public void ApplyCurrent2To()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Current2To");
#endif
        TargetTransform.localEulerAngles = rotTo;
        TargetTransform.localPosition = posTo;
        TargetTransform.localScale = scaleTo;
        if (targetRectTransform)
        {
            targetRectTransform.sizeDelta = sizeTo;
        }
    }

    [ContextMenu("SetCurrent2From")]
    public void SetCurrent2From()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Current2From");
#endif
        rotFrom = TargetTransform.localEulerAngles;
        posFrom = TargetTransform.localPosition;
        scaleFrom = TargetTransform.localScale;
        if (targetRectTransform)
        {
            sizeFrom = targetRectTransform.sizeDelta;
        }
    }
    [ContextMenu("SetCurrent2To")]
    public void SetCurrent2To()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Current2To");
#endif
        rotTo = TargetTransform.localEulerAngles;
        posTo = TargetTransform.localPosition;
        scaleTo = TargetTransform.localScale;
        if (targetRectTransform)
        {
            sizeTo = targetRectTransform.sizeDelta;
        }
    }

    protected Vector3 rotStart;
    protected Vector3 posStart;
    protected Vector3 scaleStart;
    protected Vector2 sizeStart;
    protected Vector3 rotEnd;
    protected Vector3 posEnd;
    protected Vector3 scaleEnd;
    protected Vector2 sizeEnd;

    public override void Play()
    {
        if (!TargetTransform)
        {
            base.Play();
            return;
        }
        if (animType.CheckState(AnimType.currentTransform))
        {
            this.rotStart = TargetTransform.localEulerAngles;
            this.posStart = TargetTransform.localPosition;
            this.scaleStart = TargetTransform.localScale;
            if (targetRectTransform)
            {
                this.sizeStart = targetRectTransform.sizeDelta;
            }
        }
        else
        {
            this.rotStart = rotFrom;
            this.posStart = posFrom;
            this.scaleStart = scaleFrom;
            this.sizeStart = sizeFrom;
        }
        if (animType.CheckState(AnimType.Offset))
        {
            rotEnd = rotStart + rotTo;
            posEnd = posStart + posTo;
            scaleEnd = scaleStart + scaleTo;
            sizeEnd = sizeStart + sizeTo;
        }
        else
        {
            rotEnd = rotTo;
            posEnd = posTo;
            scaleEnd = scaleTo;
            sizeEnd = sizeTo;
        }
        base.Play();
    }

    public override void ApplyTrans()
    {
        if (!TargetTransform) return;
        if (animType.ContainState(AnimType.world))
        {
            if (animType.ContainState(AnimType.rotation))
            {
                TargetTransform.localEulerAngles = Vector3.LerpUnclamped(this.rotStart, rotEnd, AnimController.Current);// Quaternion.Euler(Vector3.Lerp(rotFrom, rotTo, AnimController.Current));
            }
            if (animType.ContainState(AnimType.postion))
            {
                TargetTransform.position = Vector3.LerpUnclamped(this.posStart, posEnd, AnimController.Current);
            }
            if (animType.ContainState(AnimType.scale))
            {
                TargetTransform.localScale = Vector3.LerpUnclamped(this.scaleStart, scaleEnd, AnimController.Current);
            }
        }
        else
        {
            if (animType.ContainState(AnimType.rotation))
            {
                TargetTransform.localEulerAngles = Vector3.LerpUnclamped(this.rotStart, rotEnd, AnimController.Current);
            }
            if (animType.ContainState(AnimType.postion))
            {
                TargetTransform.localPosition = Vector3.LerpUnclamped(this.posStart, posEnd, AnimController.Current);
            }
            if (animType.ContainState(AnimType.scale))
            {
                TargetTransform.localScale = Vector3.LerpUnclamped(this.scaleStart, scaleEnd, AnimController.Current);
            }
            if (targetRectTransform)
            {
                if (animType.ContainState(AnimType.size))
                {
                    targetRectTransform.sizeDelta = Vector2.LerpUnclamped(sizeStart, sizeEnd, AnimController.Current);
                }
            }
        }
    }

}
