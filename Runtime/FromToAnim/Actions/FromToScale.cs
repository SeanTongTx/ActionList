using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class FromToScale : FromToAnim
{
    public List<Transform> targets = new List<Transform>();

    public Vector3 from;

    public Vector3 to;
    public override void ApplyTrans()
    {
        Vector3 v = Vector3.Lerp(from, to, AnimController.Current);
        foreach (Transform target in targets)
        {
            target.localScale = v;
        }
    }

    public override string Verify()
    {
        if (targets.Exists(e => e == null)) return "包含null对象";
        return string.Empty;
    }
}
