using SeanLib.Core;
using System;
using System.Collections.Generic;

using UnityEngine;

public class FromTo : MonoBehaviour
{
    public HashSet<FromToAgent> Agents = new HashSet<FromToAgent>();

    object LOCK = new object();

    private List<FromToAgent> completes = new List<FromToAgent>();
    public void Update()
    {
        lock (LOCK)
        {
            completes.Clear();
            foreach (var agent in Agents)
            {
                try
                {
                    if (agent.Update())
                    {
                        completes.Add(agent);
                    }
                }
                catch (Exception e)
                {
                    DebugConsole.Error("Fromto", "Update", e.StackTrace);
                    Agents.Remove(agent);
                    return;
                }
            }
            foreach (FromToAgent agent in completes)
            {
                Agents.Remove(agent);
                if (agent.OnComplete != null)
                {
                    agent.OnComplete();
                }
            }
        }
    }

    public void UnRegishAgent(FromToAgent agent)
    {
        agent.Reset();
        lock (LOCK)
        {
            Agents.Remove(agent);
        }
    }
    public void RegistAgent(FromToAgent agent)
    {
        agent.Reset();
        lock (LOCK)
        {
            Agents.Add(agent);
        }
    }
}
