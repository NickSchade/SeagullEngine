using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuildQueueSet
{
    Player _player;
    List<dStructurePlacement> _set;
    int _maxSetSize;
    public BuildQueueSet(Player player, int maxSetSize)
    {
        _player = player;
        _maxSetSize = maxSetSize;
        _set = new List<dStructurePlacement>();
    }
    public dStructurePlacement GetStructureAtPos(Pos p)
    {
        dStructurePlacement spd = null;
        foreach (dStructurePlacement d in _set)
        {
            if (d.location._pos == p)
            {
                spd = d;
                break;
            }
        }
        return spd;
    }
    public bool CanAddToSet(dStructurePlacement data)
    {
        if (data != null)
        {
            if (!Contains(data))
            {
                if (CanFitInSet(data))
                {
                    if (CanAffordToAddToSet(data))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
        else
        {
           return false;
        }
    }
    bool CanAffordToAddToSet(dStructurePlacement data)
    {
        float newCost = data.data.cost;
        float currentSetCost = GetSetCost();
        float totalCost = newCost + currentSetCost;
        return _player._resources.CanAfford(totalCost);
    }
    public bool CanAffordSet()
    {
        return _player._resources.CanAfford(GetSetCost());
    }
    bool CanFitInSet(dStructurePlacement data)
    {
        return _set.Count <= _maxSetSize;
    }
    public void AddToSet(dStructurePlacement data)
    {
        if (data != null)
        {
            _set.Add(data);
        }
    }
    float GetSetCost()
    {
        float cost = _set.Select(x => x.data.cost).Sum();
        return cost;
    }
    public bool Build()
    {
        if (CanAffordSet())
        {
            foreach (dStructurePlacement spd in _set)
            {
                HomelandsStructure newStructure = StructureFactory.Make(spd.location._game, spd);
                spd.location._structure = newStructure;
                _player._resources.Pay(spd.data.cost);
                _player._game._locations[spd.location._pos]._structure = newStructure;
            }
            return true;
        }
        else
        {
            Debug.Log(_player._name + " can't afford the set!");
            return false;
        }
    }
    public bool Contains(dStructurePlacement data)
    {
        return _set.Select(x => x.location).Contains(data.location);
    }
    public bool Contains(Pos p)
    {
        return _set.Select(x => x.location._pos).Contains(p);
    }
}


public class BuildQueue : IBuildQueue
{
    public Player _player;
    List<BuildQueueSet> _queue;
    int _maxQueueSize;
    public BuildQueue(Player player, int maxQueueSize = 1)
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
}
