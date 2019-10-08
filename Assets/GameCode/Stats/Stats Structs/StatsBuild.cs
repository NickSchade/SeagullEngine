using UnityEngine;
using System.Collections.Generic;

public class StatsBuild 
{
    public Dictionary<Player, dStructurePlacement> _buildingsQueued;
    public StatsBuild(Dictionary<Player, dStructurePlacement> buildingsQueued)
    {
        _buildingsQueued = buildingsQueued;
    }
}
