using System;

using UnityEngine;
using System.Collections;

public class SetActionListAndPlay : ActionBase
{
    public ActionPlayer Player;

    public ActionList list;

    public bool NeedActive = false;
    public override void Play()
    {
        base.Play();
        if (NeedActive)
        {
            if (!list.gameObject.activeInHierarchy)
            {
                this.End();
                return;
            }
        }
        if (Player.SetCurrent(list))
        {
            Player.Restart();
        }
        this.End();
    }
}
