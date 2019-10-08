using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuildQueueBrute : IBuildQueue
{
    public Player _player;
    List<BuildQueueSet> _queue;
    int _maxQueueSize;
    public BuildQueueBrute(Player player, int maxQueueSize = 1)
    {
        _player = player;
        _maxQueueSize = maxQueueSize;
        ResetQueue();
    }

    public bool TryToAddStructureToBuildQueue(dStructurePlacement data)
    {
        bool success = false;
        if (data != null)
        {
            if (_player._resources.CanAfford(data.data.cost))
            {
                int setIndex = 0;
                bool done = false;
                while (!done)
                {
                    if (_queue.Count == setIndex)
                    {
                        BuildQueueSet newBqs = new BuildQueueSet(_player, _maxQueueSize);
                        _queue.Add(newBqs);
                    }
                    BuildQueueSet bqs = _queue[setIndex];
                    if (bqs.CanAddToSet(data))
                    {
                        bqs.AddToSet(data);
                        done = true;
                        success = true;
                        Debug.Log(_player._name + " queued a structure");
                    }
                    setIndex++;
                }
            }
            else
            {
                Debug.Log(_player._name + " can't afford the structure!");
            }
        }
        return success;
    }

    public void Build()
    {
        if (_queue.Count > 0)
        {
            BuildQueueSet bqs = _queue[0];
            bool buildSuccess = bqs.Build();
            if (buildSuccess)
            {
                _queue.Remove(bqs);
            }
        }
    }

    protected void ResetQueue()
    {
        _queue = new List<BuildQueueSet>();
    }

    public dStructurePlacement GetStructureInQueueAtPos(Pos p)
    {
        dStructurePlacement spd = null;
        foreach (BuildQueueSet bqs in _queue)
        {
            dStructurePlacement spdAtPos = bqs.GetStructureAtPos(p);
            if (spdAtPos != null)
            {
                spd = spdAtPos;
                break;
            }
        }
        return spd;
    }

    public List<BuildQueueSet> GetBuildQueue()
    {
        return _queue;
    }
}
