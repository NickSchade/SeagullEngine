using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BuildQueueSet
{
    Player _player;
    int _maxSetSize;

    public List<dStructurePlacement> _set;

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
        Debug.Log("Building Set");
        if (CanAffordSet())
        {
            foreach (dStructurePlacement spd in _set)
            {
                HomelandsStructure newStructure = StructureFactory.Make(spd.location._game, spd);
                spd.location._structure = newStructure;
                _player._resources.Pay(spd.data.cost);
                _player._game._locations[spd.location._pos]._structure = newStructure;
                _player._buildQueue._queue.Remove(spd);
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

