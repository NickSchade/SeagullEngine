using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GraphicsData
{
    public Pos _pos;
    public LocationGraphicsData _location;
    public StructureGraphicsData _structure;
    public ResourceGraphicsData _resource;
    public GraphicsData(Pos pos, LocationGraphicsData location, StructureGraphicsData structure = null, ResourceGraphicsData resource = null)
    {
        _pos = pos;
        _location = location;
        _structure = structure;
        _resource = resource;
    }
}




