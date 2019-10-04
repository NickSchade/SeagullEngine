using UnityEngine;
using System.Collections.Generic;

public class MouseHandlerHomelands : IMouseHandler
{
    Dictionary<Pos, HomelandsLocation> _locations;

    public MouseHandlerHomelands(Dictionary<Pos, HomelandsLocation> locations)
    {
        _locations = locations;
    }
    public StructurePlacementData HandleMouse(MouseHandlerInfo mhi)
    {
        StructurePlacementData builtBuilding = null;
        if (mhi._pos != null)
        {
            builtBuilding = HandleBuildStructure(mhi);
        }
        return builtBuilding;
    }
    StructurePlacementData HandleBuildStructure(MouseHandlerInfo mhi)
    {
        StructurePlacementData structureToBuild = null;
        if (mhi._left[eInput.Up])
        {
            structureToBuild = _locations[mhi._pos].Click();
        }
        return structureToBuild;
    }
}