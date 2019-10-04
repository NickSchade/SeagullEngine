using UnityEngine;
using System.Collections.Generic;

public class StatsBuild 
{
    public Dictionary<Player, StructurePlacementData> _buildingsQueued;
    public StatsBuild(Dictionary<Player, StructurePlacementData> buildingsQueued)
    {
        _buildingsQueued = buildingsQueued;
    }
}
