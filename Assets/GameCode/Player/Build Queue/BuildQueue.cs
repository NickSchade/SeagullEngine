using UnityEngine;
using System.Collections.Generic;


public enum eBuildQueue { Static, Dynamic};

public interface IBuildQueue
{
    void Build();
    void AddStructureToBuildQueue(StructurePlacementData data);
}

public static class BuildQueueFactory
{
    public static IBuildQueue Make(eBuildQueue type, Player player)
    {
        if (type == eBuildQueue.Static)
        {
            return new BuildQueueStatic(player);
        }
        else if (type == eBuildQueue.Dynamic)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}


