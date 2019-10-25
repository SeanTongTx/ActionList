using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using SeanLib.Core;

public class OpenClose : ActionBase
{
    [InspectorPlus.Orderable]
    public List<GameObject> Opens = new List<GameObject>();
    [InspectorPlus.Orderable]
    public List<GameObject> Closes = new List<GameObject>();
    public override void Play()
    {
        base.Play();
        foreach (GameObject open in Opens)
        {
            open.SetActive(true);
        }
        foreach (GameObject close in Closes)
        {
            close.SetActive(false);
        }
        this.End();
    }

    public override void Revert()
    {
        foreach (GameObject open in Opens)
        {
            open.SetActive(false);
        }
        foreach (GameObject close in Closes)
        {
            close.SetActive(true);
        }
    }

    public void openClose()
    {
        foreach (GameObject open in Opens)
        {
            open.SetActive(!open.activeSelf);
        }
        foreach (GameObject close in Closes)
        {
            close.SetActive(!close.activeSelf);
        }
    }

    public override string Verify()
    {
        if (Opens.Exists(e => e == null) || Closes.Exists(e => e == null))
        {
            return "列表中包含null对象";
        }
        return string.Empty;
    }
}