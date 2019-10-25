using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSortingOrder : ActionBase
{
    public Renderer renderer;

    public SortingLayer SortingLayer;

    public int SortOrder;
    public override void Play()
    {
        base.Play();
        renderer.sortingLayerName = SortingLayer.name;
        renderer.sortingOrder = SortOrder;
        this.End();
    }

    public void OnValidate()
    {
        renderer.sortingLayerName = SortingLayer.name;
        renderer.sortingOrder = SortOrder;
    }

}
