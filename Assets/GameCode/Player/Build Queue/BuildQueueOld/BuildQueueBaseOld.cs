using UnityEngine;
using System.Collections.Generic;


public abstract class BuildQueueBaseOld : IBuildQueue
{
    public Player _player;
    public List<dStructurePlacement> _queue;

    public BuildQueueBaseOld(Player player)
    {
        _player = player;
        ResetBuildQueue();
    }


    public bool TryToAddStructureToBuildQueue(dStructurePlacement data)
    {
        bool success = false;
        if (data != null)
        {
            if (CanAffordToAddToQueue(data))
            {
                _queue.Remove(GetStructureAlreadInQueue(data));
                _queue.Add(data);
                success = true;
            }
            else
            {
                Debug.Log("can't afford to add to queue");
            }
        }
        return success;
    }
    public void Build()
    {
        if (CanAffordToBuildQueue())
        {
            Debug.Log("Building Player Queue");
            BuildQueue();
        }
        else
        {
            Debug.Log("Can't Afford Queue; Purging");
            DoIfCantAffordToBuild();
        }
    }
    
    protected float GetTotalCost()
    {
        float cost = 0f;
        foreach (dStructurePlacement data in _queue)
        {
            cost += data.data.cost;
        }
        return cost;
    }
    protected abstract bool CanAffordToAddToQueue(dStructurePlacement data);
    public bool CanAffordEntireQueuePlusNewStructure(dStructurePlacement data)
    {
        float currentCost = GetTotalCost();
        float totalCost = currentCost + data.data.cost;
        return _player._resources.CanAfford(totalCost);
    }
    protected bool CanAddToLimitlessQueue()
    {
        return true;
    }
    protected bool CanAffordNextInQueue()
    {
        if (_queue.Count > 0)
        {
            dStructurePlacement spd = _queue[0];
            return _player._resources.CanAfford(spd.data.cost);
        }
        else
        {
            return true;
        }
    }
    protected abstract bool CanAffordToBuildQueue();
    protected bool CanAffordEntireQueue()
    {
        float currentCost = GetTotalCost();
        return _player._resources.CanAfford(currentCost); 
    }

    protected dStructurePlacement GetStructureAlreadInQueue(dStructurePlacement data)
    {
        dStructurePlacement spd = null;
        foreach (dStructurePlacement d in _queue)
        {
            if (d.location == data.location)
            {
                spd = d;
                break;
            }
        }

        return spd;
    }
    public dStructurePlacement GetStructureToPlace(dStructure structureToBuild, HomelandsLocation buildLocation)
    {
        dStructurePlacement placementData = new dStructurePlacement(structureToBuild, _player, buildLocation);
        if (CanAffordEntireQueuePlusNewStructure(placementData))
        {
            return placementData;
        }
        else
        {
            Debug.Log($"{_player._name} can't Build - Insufficient Resources");
            return null;
        }
    }



    protected abstract void DoIfCantAffordToBuild();

    protected void ResetBuildQueue()
    {
        _queue = new List<dStructurePlacement>();
    }

    protected abstract void BuildQueue();

    protected void BuildEntireQueue()
    {
        List<dStructurePlacement> dataInQueue = GetDataInQueue();
        foreach (dStructurePlacement spd in dataInQueue)
        {
            HomelandsStructure newStructure = StructureFactory.Make(spd.location._game, spd);
            spd.location._structure = newStructure;
            _player._resources.Pay(spd.data.cost);
        }
        ResetBuildQueue();
    }


    protected void BuildNextInQueue()
    {
        if (_queue.Count > 0)
        {
            dStructurePlacement spd = _queue[0];
            HomelandsStructure newStructure = StructureFactory.Make(spd.location._game, spd);
            spd.location._structure = newStructure;
            _player._resources.Pay(spd.data.cost);
            _queue.Remove(spd);
        }
    }
    protected List<dStructurePlacement> GetDataInQueue()
    {
        return _queue;
    }
    public dStructurePlacement GetStructureInQueueAtPos(Pos p)
    {
        dStructurePlacement spd = null;
        List<dStructurePlacement> q = GetDataInQueue();
        foreach (dStructurePlacement d in q)
        {
            if (d.location == _player._game._locations[p])
            {
                spd = d;
            }
        }
        return spd;
    }
}
