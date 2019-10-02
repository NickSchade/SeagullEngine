using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MapBuilderGaea : MapBuilderBase, IMapBuilder
{

    public MapBuilderGaea(HomelandsGame game)
    {
        _game = game;
    }

    public Dictionary<Pos, HomelandsLocation> Make()
    {
        Debug.Log("Making Map");

        MapSettings settings = new MapSettings(_game._tileShape, 19, 19, false, false);

        GaeaWorldbuilder map = new GaeaWorldbuilder(settings.xDim, settings.yDim);
        map.GenerateMap();

        IMapLocSetter mapLocSetter = MapLocSetterFactory.Make(_game._tileShape);

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
                locations[p] = LocationFactory.Make(_game, p, locationQualities);
            }
        }
        return locations;
    }


    
}
