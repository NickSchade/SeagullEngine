using UnityEngine;
using System.Collections;

public class BuildQueueDynamicOld : BuildQueueBaseOld, IBuildQueue
{
    public BuildQueueDynamicOld(Player player) : base(player)
    {
    }

    protected override void BuildQueue()
    {
        BuildNextInQueue();
    }

    protected override bool CanAffordToAddToQueue(dStructurePlacement data)
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
