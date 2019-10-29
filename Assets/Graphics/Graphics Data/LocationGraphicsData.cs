using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGraphicsData
{
    // class instead of struct for parity with StructureGraphicsData
    Color _color;
    public LocationGraphicsData(Color color)
    {
        _color = color;
    }
    public Color GetColor()
    {
        return _color;
    }
}
