using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGraphicsData
{
    Color _color;
    public ResourceGraphicsData(Color color)
    {
        _color = color;
    }
    public Color GetColor()
    {
        return _color;
    }
}
