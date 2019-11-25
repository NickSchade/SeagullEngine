using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class MapBuilderBase
{
    public HomelandsGame _game;
    public eTileShape _tileShape;

    public Dictionary<Pos, HomelandsLocation> SetNeighbors(MapSettings settings, Dictionary<Pos, HomelandsLocation> locations)
    {
        Dictionary<string, Pos> stringMap = GetStringMap(locations);
        MapInfo mapInfo = new MapInfo(_game, locations, settings, stringMap);
        NeighborBuilder nb = NeighborBuilderFactory.Make(mapInfo);
        foreach (string k in stringMap.Keys)
        {
            nb.SetNeighbors(stringMap[k]);
        }
        return locations;
    }

    public Dictionary<string, Pos> GetStringMap(Dictionary<Pos, HomelandsLocation> locations)
    {
        Dictionary<string, Pos> stringMap = new Dictionary<string, Pos>();

        foreach (Pos p in locations.Keys)
        {
            stringMap[p._gridLoc.key()] = p;
        }
        return stringMap;
    }
}