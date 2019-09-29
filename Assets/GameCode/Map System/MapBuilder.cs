using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMap { Simple};

public static class MapBuilder
{

    public static Dictionary<Pos, HomelandsLocation> Make(HomelandsGame game)
    {
        Debug.Log("Making Map");

        MapGen map = new MapGen(16, 16);
        map.GenerateMap();

        IMapLocSetter mapLocSetter = MapLocSetterFactory.Make(game._tileShape);

        Dictionary<Pos, HomelandsLocation> locations = BuildLocations(game, map, mapLocSetter);

        SetNeighbors(game, locations);

        Debug.Log("Made Map");

        return locations;
    }
    static Dictionary<Pos,HomelandsLocation> BuildLocations(HomelandsGame game, MapGen map, IMapLocSetter mapLocSetter)
    {
        Dictionary<Pos, HomelandsLocation> locations = new Dictionary<Pos, HomelandsLocation>();
        for (int x = 0; x < map.xDim; x++)
        {
            for (int y = 0; y < map.yDim; y++)
            {
                Loc l = new Loc(x, y);
                Pos p = new Pos(l, mapLocSetter);
                Dictionary<string, float> locationQualities = map.GetLocationQualities(x, y);
                locations[p] = LocationFactory.Make(game, p, locationQualities);
            }
        }
        return locations;
    }
    public static Dictionary<Pos,HomelandsLocation> SetNeighbors(HomelandsGame game, Dictionary<Pos,HomelandsLocation> locations)
    {
        Dictionary<string, Pos> stringMap = GetStringMap(locations);
        MapInfo mapInfo = new MapInfo(game, locations, game._tileShape, 16, 16, false, false, stringMap);
        NeighborBuilder nb = NeighborBuilderFactory.Make(mapInfo);
        foreach (string k in stringMap.Keys)
        {
            nb.SetNeighbors(stringMap[k]);
        }
        return locations;
    }

    public static Dictionary<string, Pos> GetStringMap(Dictionary<Pos, HomelandsLocation> locations)
    {
        Dictionary<string, Pos> stringMap = new Dictionary<string, Pos>();

        foreach (Pos p in locations.Keys)
        {
            stringMap[p.gridLoc.key()] = p;
        }
        return stringMap;
    }


    
}
