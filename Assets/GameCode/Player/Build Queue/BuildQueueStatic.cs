using UnityEngine;
using System.Collections.Generic;

public class BuildQueueStatic : BuildQueueBase, IBuildQueue
{
    public BuildQueueStatic(Player player) : base(player)
    {
    }
    

    protected override bool CanAffordToAddToQueue(StructurePlacementData data)
    {
        return CanAffordEntireQueuePlusNewStructure(data);
    }

    protected override bool CanAffordToBuildQueue()
    {
        return CanAffordEntireQueue();
    }

    protected override void DoIfCantAffordToBuild()
    {
        ResetBuildQueue();
    }

    protected override void BuildQueue()
    {
        BuildEntireQueue();
    }
}