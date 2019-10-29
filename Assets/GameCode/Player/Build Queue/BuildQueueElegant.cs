using UnityEngine;
using System.Collections.Generic;


public class BuildQueueElegant : IBuildQueue
{
    public Player _player;
    List<dStructurePlacement> _queue;
    int _maxQueueSize;
    public BuildQueueElegant(Player player, int maxQueueSize = 1)
    {
        _player = player;
        _maxQueueSize = maxQueueSize;
        ResetQueue();
    }

    public List<BuildQueueSet> GetBuildQueue()
    {
        List<BuildQueueSet> returnQueue = new List<BuildQueueSet>();
        int index = 0;
        BuildQueueSet bqs = new BuildQueueSet(_player, _maxQueueSize);
        while (index < _queue.Count)
        {
            dStructurePlacement dsp = _queue[index];
            if (!bqs.CanAddToSet(dsp))
            {
                returnQueue.Add(bqs);
                bqs = new BuildQueueSet(_player, _maxQueueSize);
            }
            if (bqs.CanAddToSet(dsp))
            {
                bqs.AddToSet(dsp);
            }
            index++;
        }
        if (returnQueue.Count > 0 && returnQueue[returnQueue.Count - 1] != bqs)
        {
            returnQueue.Add(bqs);
        }
        return returnQueue;
    }

    public void Build()
    {
        List<BuildQueueSet> setQueue = GetBuildQueue();
        if (setQueue.Count > 0)
        {
            BuildQueueSet bqs = setQueue[0];
            bqs.Build();
        }
    }

    public dStructurePlacement GetStructureInQueueAtPos(Pos p)
    {
        dStructurePlacement dsp = null;
        foreach (dStructurePlacement q in _queue)
        {
            if (q.location._pos == p)
            {
                dsp = q;
                break;
            }
        }
        return dsp;
    }

    public bool TryToAddStructureToBuildQueue(dStructurePlacement data)
    {
        bool success = false;
        if (data != null)
        {
            if (_player._resources.CanAfford(data.data.cost))
            {
                success = true;
                _queue.Add(data);
            }
            else
            {
                Debug.Log(_player._name + " can't afford the structure!");
            }
        }
        return success;
    }

    void ResetQueue()
    {
        _queue = new List<dStructurePlacement>();
    }
}

