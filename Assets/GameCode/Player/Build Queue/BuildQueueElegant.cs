using UnityEngine;
using System.Collections.Generic;


public class BuildQueueElegant
{
    public Player _player;
    public List<dStructurePlacement> _queue;
    int _maxQueueSize;
    public BuildQueueElegant(Player player, int maxQueueSize = 1)
    {
        _player = player;
        _maxQueueSize = maxQueueSize;
        ResetQueue();
    }

    List<BuildQueueSet> GetBuildQueue()
    {
        List<BuildQueueSet> returnQueue = new List<BuildQueueSet>();
        BuildQueueSet bqs = new BuildQueueSet(_player, _maxQueueSize);
        //Debug.Log($"Queue has {_queue.Count}");
        foreach (dStructurePlacement dsp in _queue)
        {
            if (!bqs.CanAddToSet(dsp))
            {
                returnQueue.Add(bqs);
                bqs = new BuildQueueSet(_player, _maxQueueSize);
            }
            if (bqs.CanAddToSet(dsp))
            {
                bqs.AddToSet(dsp);
            }
        }
        if (!returnQueue.Contains(bqs))
        {
            returnQueue.Add(bqs);
        }
        //Debug.Log($"There are {returnQueue.Count} sets in the queue");
        //for (int i = 0; i < returnQueue.Count; i++)
        //{
        //    Debug.Log($"{returnQueue[i]._set.Count} in set {i}");
        //}
        return returnQueue;
    }

    public void Build()
    {
        List<BuildQueueSet> setQueue = GetBuildQueue();
        if (setQueue.Count > 0)
        {
            BuildQueueSet bqs = setQueue[0];
            bool built = bqs.Build();
            Debug.Log("Built " + (built ? "successfully" : "uncusessfully"));
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

