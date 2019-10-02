using UnityEngine;
using System.Collections.Generic;

public class MouseHandlerHomelands : IMouseHandler
{
    Dictionary<Pos, HomelandsLocation> _locations;

    public MouseHandlerHomelands(Dictionary<Pos, HomelandsLocation> locations)
    {
        _locations = locations;
    }
    public bool HandleMouse(MouseHandlerInfo mhi)
    {
        if (mhi._pos != null)
        {
            if (mhi._left[eInput.Up])
            {
                _locations[mhi._pos].Click();
            }
            if (mhi._right[eInput.Up])
            {
                //HomelandsStructure structure = _locations[mhi._pos]._structure;
                //if (structure != null)
                //{
                //    structure.DestroyThis();
                //}
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}