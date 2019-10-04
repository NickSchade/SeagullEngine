using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGraphicsData
{
    // class instead of struct so it can be nullable
    Color _color;
    public StructureGraphicsData(Color color)
    {
        _color = color;
    }
    public Color GetColor()
    {
        Color c = _color;
        return c;
    }
}
