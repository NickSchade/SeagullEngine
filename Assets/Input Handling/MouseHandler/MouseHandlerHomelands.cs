using UnityEngine;
using System.Collections.Generic;

public class MouseHandlerHomelands
{
    Dictionary<Pos, HomelandsLocation> _locations;

    public MouseHandlerHomelands(Dictionary<Pos, HomelandsLocation> locations)
    {
        _locations = locations;
    }
    public MouseHandlerOutput GetMouseHandlerOutput(MouseHandlerInfo mhi)
    {
        dStructurePlacement builtBuilding = GetStructurePlacement(mhi);
        SelectedData selected = GetSelectedData(mhi);
        MouseHandlerOutput mho = new MouseHandlerOutput();
        mho._structurePlaced = builtBuilding;
        mho._selected = selected;

        return mho;
    }
    dStructurePlacement GetStructurePlacement(MouseHandlerInfo mhi)
    {
        dStructurePlacement structureToBuild = null;
        if (mhi._pos != null && mhi._left[eInput.Up])
        {
            structureToBuild = _locations[mhi._pos].Click();
        }
        return structureToBuild;
    }
    SelectedData GetSelectedData(MouseHandlerInfo mhi)
    {
        SelectedData sd = null;
        if (mhi._pos != null)
        {
            HomelandsLocation loc = _locations[mhi._pos];
            sd = new SelectedData(loc);
        }
        return sd;
    }
}