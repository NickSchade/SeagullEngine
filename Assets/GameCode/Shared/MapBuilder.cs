using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMap { Simple};

public static class MapBuilder
{

    public static Dictionary<Pos, ILocation> Make(HomelandsGame game)
    {
        Debug.Log("Making Map");

        MapGen map = new MapGen(16, 16);
        map.GenerateMap();

        Dictionary<Pos, ILocation> locations = new Dictionary<Pos, ILocation>();

        for (int x = 0; x < map.xDim; x++)
        {
            for (int y = 0; y < map.yDim; y++)
            {
                Loc l = new Loc(x, y);
                Pos p = new Pos(l, game._tileShape);
                locations[p] = LocationFactory.Make(p, game._gameType, game._locationDrawer);
            }
        }

        Debug.Log("Made Map");

        return locations;
    }

    public static Dictionary<string, Pos> GetStringMap(Dictionary<Pos,ILocation> locations)
    {
        Dictionary<string, Pos> stringMap = new Dictionary<string, Pos>();

        foreach (Pos p in locations.Keys)
        {
            stringMap[p.gridLoc.key()] = p;
        }
        return stringMap;
    }
    
}
