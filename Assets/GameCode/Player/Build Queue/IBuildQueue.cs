using UnityEngine;
using System.Collections.Generic;


public enum eBuildQueue { Brute, Elegant };

public interface IBuildQueue
{
    void Build();
    bool TryToAddStructureToBuildQueue(dStructurePlacement data);
    dStructurePlacement GetStructureInQueueAtPos(Pos p);
    List<BuildQueueSet> GetBuildQueue();
}



