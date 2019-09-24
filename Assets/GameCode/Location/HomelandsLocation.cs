using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HomelandsLocation
{
    public Pos _pos;
    HomelandsGame _game;

    public HomelandsStructure _structure { get; set; }
    public Viewer _viewer { get; set; }
    public eTerrain _terrain;


    public HomelandsLocation(HomelandsGame game, Pos pos, Dictionary<string, float> locationQualities)
    {
        _pos = pos;
        _game = game;
        _viewer = game._viewer;

        _terrain = GetTerrainFromQualities(locationQualities);
    }
    eTerrain GetTerrainFromQualities(Dictionary<string, float> locationQualities)
    {
        float elevation = locationQualities["Elevation"];
        if (elevation < 0.4)
        {
            return eTerrain.Sea;
        }
        else if (elevation < 0.8)
        {
            return eTerrain.Land;
        }
        else
        {
            return eTerrain.Mountain;
        }
    }

    public virtual void TryToMakeStructure()
    {
        if (_structure == null)
        {
            _structure = StructureFactory.Make(_game, this);
        }
        else
        {
            Debug.Log("Can't Build");
        }
    }
    public virtual void Click()
    {
        TryToMakeStructure();
    }

    public virtual GraphicsData Draw(HomelandsPosStats stats)
    {
        LocationGraphicsData lgd = GetLocationData(stats);
        StructureGraphicsData sgd = GetStructureData(stats);

        GraphicsData gd = new GraphicsData(_pos, lgd, sgd);

        return gd;
    }

    StructureGraphicsData GetStructureData(HomelandsPosStats stats)
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
    LocationGraphicsData GetLocationData(HomelandsPosStats stats)
    {
        return _viewer.Draw(this, stats);
    }
}

