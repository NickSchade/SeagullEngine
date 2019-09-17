using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsLocation : ILocation
{
    public IStructure _structure { get; set; }
    public ILocationDrawer _drawer { get; set; }

    public Pos _pos;
    public HomelandsLocation(Pos pos, ILocationDrawer locationDrawer) 
    {
        _pos = pos;
        _drawer = locationDrawer;
    }
    public void Click()
    {
        Debug.Log("Clicked HomelandsLocation at " + _pos.getName());
    }

    public GraphicsData Draw()
    {
        LocationGraphicsData lgd = GetLocationData();
        StructureGraphicsData sgd = GetStructureData();
        GraphicsData gd = new GraphicsData(_pos, lgd, sgd);
        return gd;
    }

    StructureGraphicsData GetStructureData()
    {
        if (_structure == null)
        {
            return null;
        }
        else
        {
            return _structure.Draw();
        }
    }

    public LocationGraphicsData GetLocationData()
    {
        return _drawer.Draw();
    }
}

public class HomelandsSandboxLocation : HomelandsLocation, ILocation
{
    public HomelandsSandboxLocation(Pos pos, ILocationDrawer locationDrawer) : base(pos, locationDrawer)
    {
    }
}
