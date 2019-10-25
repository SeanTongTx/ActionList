using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FromtoClips : FromToAnim
{
    public List<Sprite> SpritesClips = new List<Sprite>();

    public Image View;

    public SpriteRenderer spriteRenderer;
    public override void ApplyTrans()
    {
        int index = (int)(this.AnimController.Current / this.AnimController.to * SpritesClips.Count);
        if (index < SpritesClips.Count)
        {
            if (View != null)
                View.sprite = SpritesClips[index];
            if (spriteRenderer)
            {
                spriteRenderer.sprite = SpritesClips[index];
            }
        }
    }
}
