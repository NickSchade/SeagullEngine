using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilderGaea : MapBuilderBase, IMapBuilder
{
    public MapBuilderGaea(HomelandsGame game)
    {
        _game = game;
    }

    public Dictionary<Pos, HomelandsLocation> Make(MapSettings settings)
    {
        Debug.Log("Making Map");


        GaeaWorldbuilder map = new GaeaWorldbuilder(settings.xDim, settings.yDim);
        map.GenerateMap();

        IMapLocSetter mapLocSetter = MapLocSetterFactory.Make(_game._settings._mapSettings._tileShape);

        Dictionary<Pos, HomelandsLocation> locations = BuildLocations(map, mapLocSetter);


        SetNeighbors(settings, locations);

        Debug.Log("Made Map");

        return locations;
    }
    Dictionary<Pos,HomelandsLocation> BuildLocations(GaeaWorldbuilder map, IMapLocSetter mapLocSetter)
    {
        Dictionary<Pos, HomelandsLocation> locations = new Dictionary<Pos, HomelandsLocation>();
        for (int x = 0; x < map.xDim; x++)
        {
            for (int y = 0; y < map.yDim; y++)
            {
                Loc l = new Loc(x, y);
                Pos p = new Pos(l, mapLocSetter);
                Dictionary<string, float> locationQualities = map.GetLocationQualities(x, y);
                HomelandsTerrain terrain = GetTerrain(locationQualities);
                HomelandsResource resource = terrain._type == eTerrain.Land && Random.Range(0f, 1f) > 0.90f ? new HomelandsResource() : null;
                locations[p] = LocationFactory.Make(_game, p, terrain, resource);
            }
        }
        return locations;
    }

    HomelandsTerrain GetTerrain(Dictionary<string, float> locationQualities)
    {
        eTerrain type = GetTerrainTypeFromQualities(locationQualities);
        HomelandsTerrain terrain = new HomelandsTerrain(type);
        return terrain;
    }

    eTerrain GetTerrainTypeFromQualities(Dictionary<string, float> locationQualities)
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


}
