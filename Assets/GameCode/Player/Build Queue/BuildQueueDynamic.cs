using UnityEngine;
using System.Collections;

public class BuildQueueDynamic : BuildQueueBase, IBuildQueue
{
    public BuildQueueDynamic(Player player) : base(player)
    {
    }

    protected override void BuildQueue()
    {
        BuildNextInQueue();
    }

    protected override bool CanAffordToAddToQueue(StructurePlacementData data)
    {
        return CanAddToLimitlessQueue();
    }

    protected override bool CanAffordToBuildQueue()
    {
        return CanAffordNextInQueue();
    }

    protected override void DoIfCantAffordToBuild()
    {
        // DONT DO ANYTHING I GUESS?
        // MAYBE INSTEAD WE SHOULD REMOVE THE 1st SPD FROM THE QUEUE?
        // MAYBE INSTEAD WE SHOULD REMOVE THE 1st SPD FROM THE QUEUE AND ITERATE?
    }
}
