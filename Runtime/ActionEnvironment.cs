using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEnvironment
{
    /// <summary>
    /// The cache.
    /// </summary>
    private Dictionary<string, object> Caches = new Dictionary<string, object>();

    /// 常用对象
    public object ActionObject
    {
        get { return Read("ActionObject"); }
        set
        {
            Cache("ActionObject",value);
        }
    }

    public object EffectTarget
    {
        get { return Read("EffectTarget"); }
        set
        {
            Cache("EffectTarget", value);
        }
    }

    public void Cache(string name, object obj)
    {
        Caches[name] = obj;
    }

    public void UnCache(string name)
    {
        Caches.Remove(name);
    }

    public object Read(string name)
    {
        object obj = null;
        Caches.TryGetValue(name, out obj);
        return obj;
    }

    public void Clear()
    {
        Caches.Clear();
    }
}