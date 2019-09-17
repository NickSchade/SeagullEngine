using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface IMouseHandler
{
    bool HandleMouse(MouseHandlerInfo mouseHandlerInfo);
}
public class MouseHandlerHomelands : IMouseHandler
{
    Dictionary<Pos, ILocation> _locations;
    public MouseHandlerHomelands(Dictionary<Pos,ILocation> locations)
    {
        _locations = locations;
    }
    public bool HandleMouse(MouseHandlerInfo mhi)
    {
        if (mhi._pos != null)
        {
            _locations[mhi._pos].Click();
            return true;
        }
        else
        {
            return false;
        }
    }
}
