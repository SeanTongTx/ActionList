using SeanLib.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FromToColor : FromToAnim
{
    /// <summary>
    /// The graphics.
    /// </summary>
    [InspectorPlus.Orderable]
    public List<Graphic> graphics = new List<Graphic>();
    [InspectorPlus.Orderable]
    public List<SpriteRenderer> Renderers = new List<SpriteRenderer>(); 

    public Color from = Color.white;

    public Color to = Color.white;

    public bool AlphaOnly;
    public bool AlphaActive;

    public override void Reset()
    {
        base.Reset();
        foreach (Graphic graphic in graphics)
        {
            if (graphic != null)
            {
                if (this.AlphaActive)
                {
                    graphic.gameObject.SetActive(true);
                }
            }
        }
        foreach (SpriteRenderer rend in Renderers)
        {
            if (rend != null)
            {
                if (this.AlphaActive)
                {
                    rend.gameObject.SetActive(true);
                }
            }
        }
    }

    public override void ApplyTrans()
    {
        foreach (Graphic graphic in graphics)
        {
            if (AlphaOnly)
            {
                Color c = graphic.color;
                c.a = Mathf.Lerp(from.a, to.a, AnimController.Current);
                graphic.color = c;
            }
            else
            {
                graphic.color = Color.Lerp(from, to, AnimController.Current);
            }
            graphic.SetMaterialDirty();
        }
        foreach (SpriteRenderer rend in Renderers)
        {
            if (AlphaOnly)
            {
                Color c = rend.material.color;
                c.a = Mathf.Lerp(from.a, to.a, AnimController.Current);
                rend.material.color = c;
            }
            else
            {
                rend.material.color = Color.Lerp(from, to, AnimController.Current);
            }
        }
    }

    public override void Play()
    {

        base.Play();
        foreach (Graphic graphic in graphics)
        {
            if (this.AlphaActive)
            {
                if (graphic.color.a <= 0.01f)
                {
                    graphic.gameObject.SetActive(true);
                }
            }
        }
        foreach (SpriteRenderer rend in Renderers)
        {
            if (this.AlphaActive)
            {
                if (rend.material.color.a <= 0.01f)
                {
                    rend.gameObject.SetActive(true);
                }
            }
        }
    }

    public override void End()
    {
        foreach (Graphic graphic in graphics)
        {
            if (this.AlphaActive)
            {
                if (graphic.color.a <= 0.01f)
                {
                    graphic.gameObject.SetActive(false);
                }
            }
        }
        foreach (SpriteRenderer rend in Renderers)
        {
            if (this.AlphaActive)
            {
                if (rend.material.color.a <= 0.01f)
                {
                    rend.gameObject.SetActive(false);
                }
            }
        }
        base.End();
    }

    public override string Verify()
    {
        if (graphics.Exists(e => e == null)) return "包含null对象";
        return string.Empty;
    }
#if UNITY_EDITOR
    [ContextMenu("Apply2From")]

    public void Apply2From()
    {
        foreach (Graphic graphic in graphics)
        {
            graphic.color = this.from;
            graphic.SetMaterialDirty();
        }

        foreach (SpriteRenderer rend in Renderers)
        {
            rend.material.color = this.from;
        }
    }
    [ContextMenu("Apply2To")]
    public void Apply2To()
    {
        foreach (Graphic graphic in graphics)
        {
            graphic.color = this.to;
            graphic.SetMaterialDirty();
        }
        foreach (SpriteRenderer rend in Renderers)
        {
            rend.material.color = this.to;
        }
    }

    [ContextMenu("reversal")]
    public void reversal()
    {
        Color t = from;
        from = to;
        to = t;
    }
#endif
}

