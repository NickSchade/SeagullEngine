using UnityEngine;
using System.Collections.Generic;


public abstract class BuildQueueBase : IBuildQueue
{
    public Player _player;
    public List<StructurePlacementData> _queue;

    public BuildQueueBase(Player player)
    {
        _player = player;
        ResetBuildQueue();
    }


    public void AddStructureToBuildQueue(StructurePlacementData data)
    {
        if (data != null)
        {
            if (CanAffordToAddToQueue(data))
            {
                _queue.Remove(GetStructureAlreadInQueue(data));
                _queue.Add(data);
            }
            else
            {
                Debug.Log("can't afford to add to queue");
            }
        }
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
        foreach (StructurePlacementData data in _queue)
        {
            cost += data.data.cost;
        }
        return cost;
    }
    protected abstract bool CanAffordToAddToQueue(StructurePlacementData data);
    protected bool CanAffordEntireQueuePlusNewStructure(StructurePlacementData data)
    {
        float currentCost = GetTotalCost();
        float totalCost = currentCost + data.data.cost;
        return CanAffordAmount(totalCost);
    }
    protected bool CanAddToLimitlessQueue()
    {
        return true;
    }
    protected bool CanAffordNextInQueue()
    {
        if (_queue.Count > 0)
        {
            StructurePlacementData spd = _queue[0];
            return CanAffordAmount(spd.data.cost);
        }
        else
        {
            return true;
        }
    }
    protected bool CanAffordAmount(float amount)
    {
        return amount <= _player._resources._resource;
    }
    protected abstract bool CanAffordToBuildQueue();
    protected bool CanAffordEntireQueue()
    {
        float currentCost = GetTotalCost();
        return CanAffordAmount(currentCost);
    }

    protected StructurePlacementData GetStructureAlreadInQueue(StructurePlacementData data)
    {
        StructurePlacementData spd = null;
        foreach (StructurePlacementData d in _queue)
        {
            if (d.location == data.location)
            {
                spd = d;
                break;
            }
        }

        return spd;
    }
    public StructurePlacementData GetStructureToPlace(StructureData structureToBuild, HomelandsLocation buildLocation)
    {
        StructurePlacementData placementData = new StructurePlacementData(structureToBuild, _player, buildLocation);
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
        _queue = new List<StructurePlacementData>();
    }

    protected abstract void BuildQueue();

    protected void BuildEntireQueue()
    {
        List<StructurePlacementData> dataInQueue = GetDataInQueue();
        foreach (StructurePlacementData spd in dataInQueue)
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
            StructurePlacementData spd = _queue[0];
            HomelandsStructure newStructure = StructureFactory.Make(spd.location._game, spd);
            spd.location._structure = newStructure;
            _player._resources.Pay(spd.data.cost);
            _queue.Remove(spd);
        }
    }
    protected List<StructurePlacementData> GetDataInQueue()
    {
        return _queue;
    }
    public StructurePlacementData GetStructureInQueueAtPos(Pos p)
    {
        StructurePlacementData spd = null;
        List<StructurePlacementData> q = GetDataInQueue();
        foreach (StructurePlacementData d in q)
        {
            if (d.location == _player._game._locations[p])
            {
                spd = d;
            }
        }
        return spd;
    }
}
