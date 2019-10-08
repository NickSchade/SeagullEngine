using UnityEngine;
using System.Collections.Generic;

public class BuildQueueStaticOld : BuildQueueBaseOld, IBuildQueue
{
    public BuildQueueStaticOld(Player player) : base(player)
    {
    }
    

    protected override bool CanAffordToAddToQueue(dStructurePlacement data)
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