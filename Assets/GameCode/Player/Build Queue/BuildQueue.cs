using UnityEngine;
using System.Collections.Generic;


public enum eBuildQueue { DynamicOld, StaticOld, Better };

public interface IBuildQueue
{
    void Build();
    bool TryToAddStructureToBuildQueue(dStructurePlacement data);
    dStructurePlacement GetStructureInQueueAtPos(Pos p);
}

public static class BuildQueueFactory
{
    public static IBuildQueue Make(eBuildQueue type, Player player)
    {
        if (type == eBuildQueue.StaticOld)
        {
            return new BuildQueueStaticOld(player);
        }
        else if (type == eBuildQueue.DynamicOld)
        {
            return new BuildQueueDynamicOld(player);
        }
        else if (type == eBuildQueue.Better)
        {
            return new BuildQueue(player);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}


