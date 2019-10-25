using UnityEngine;

public class ActionPack : ActionBase
{
    public override Color ActionColor
    {
        get
        {
            return Color.black;
        }
    }

    public bool immdiately;
    public ActionPlayer PackPlayer;
    public ActionList PackActions;
    [ContextMenu("Play")]
    public override void Play()
    {
        if (this.PackPlayer)
        {
            if (this.PackPlayer.SetCurrent(this.PackActions, this.End, immdiately))
            {
                base.Play();
                this.PackPlayer.Restart();
            }
        }
    }

    public static ActionPack New()
    {
        GameObject pack = new GameObject { name = "ActionPack" };
        ActionPack actionpack = pack.AddComponent<ActionPack>();
        ActionPlayer player = pack.AddComponent<ActionPlayer>();
        ActionList list = pack.AddComponent<ActionList>();
        actionpack.PackPlayer = player;
        actionpack.PackActions = list;
        actionpack.ActionName = "动作套件";
        return actionpack;
    }

    public override void Stop()
    {
        base.Stop();
        PackPlayer.Stop();
    }
}