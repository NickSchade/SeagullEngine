using UnityEngine;
using System.Collections.Generic;

public class MouseHandlerHomelands : IMouseHandler
{
    Dictionary<Pos, HomelandsLocation> _locations;

    public MouseHandlerHomelands(Dictionary<Pos, HomelandsLocation> locations)
    {
        _locations = locations;
    }
    public dStructurePlacement HandleMouse(MouseHandlerInfo mhi)
    {
        dStructurePlacement builtBuilding = null;
        if (mhi._pos != null)
        {
            builtBuilding = HandleBuildStructure(mhi);
        }
        return builtBuilding;
    }
    dStructurePlacement HandleBuildStructure(MouseHandlerInfo mhi)
    {
        dStructurePlacement structureToBuild = null;
        if (mhi._left[eInput.Up])
        {
            structureToBuild = _locations[mhi._pos].Click();
        }
        return structureToBuild;
    }
}