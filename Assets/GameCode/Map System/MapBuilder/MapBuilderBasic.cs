using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBuilderBasic : MapBuilderBase, IMapBuilder
{
    public MapBuilderBasic(HomelandsGame game)
    {
        _game = game;
    }
    public Dictionary<Pos, HomelandsLocation> Make(MapSettings mapSettings)
    {

        IMapLocSetter mapLocSetter = MapLocSetterFactory.Make(mapSettings.tileShape);

        Dictionary<Pos, HomelandsLocation> locations = BuildLocations(mapSettings, mapLocSetter);
        SetNeighbors(mapSettings, locations);

        return locations;
    }

    Dictionary<Pos, HomelandsLocation> BuildLocations(MapSettings mapSettings, IMapLocSetter mapLocSetter)
    {
        Dictionary<Pos, HomelandsLocation> locations = new Dictionary<Pos, HomelandsLocation>();
        for (int x = 0; x < mapSettings.xDim; x++)
        {
            for (int y = 0; y < mapSettings.yDim; y++)
            {
                Loc l = new Loc(x, y);
                Pos p = new Pos(l, mapLocSetter);
                HomelandsTerrain terrain = new HomelandsTerrain(eTerrain.Land);
                HomelandsResource resource = GetResource(x, y);
                locations[p] = LocationFactory.Make(_game, p, terrain, resource);
            }
        }
        return locations;
    }

    HomelandsResource GetResource(int x, int y)
    {
        List<int> starPoints = new List<int> { 0, 3,9,15,18 };
        if (starPoints.Contains(x) && starPoints.Contains(y))
        {
            //Debug.Log($"Star Point at {x},{y}");
            return new HomelandsResource();
        }
        else
        {
           // Debug.Log($"No Star Point at {x},{y}");
            return null;
        }
    }
}
