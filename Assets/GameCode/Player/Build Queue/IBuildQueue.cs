using UnityEngine;
using System.Collections.Generic;


public enum eBuildQueue { DynamicOld, StaticOld, Brute, Elegant };

public interface IBuildQueue
{
    void Build();
    bool TryToAddStructureToBuildQueue(dStructurePlacement data);
    dStructurePlacement GetStructureInQueueAtPos(Pos p);
    List<BuildQueueSet> GetBuildQueue();
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
        else if (type == eBuildQueue.Brute)
        {
            return new BuildQueueBrute(player);
        }
        else if (type == eBuildQueue.Elegant)
        {
            return new BuildQueueElegant(player);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}


