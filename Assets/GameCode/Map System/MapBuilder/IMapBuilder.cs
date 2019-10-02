using UnityEngine;
using System.Collections.Generic;

public interface IMapBuilder 
{
    Dictionary<Pos, HomelandsLocation> Make();
}

public enum eMap { Gaea };

public static class MapBuilderFactory
{
    public static IMapBuilder Make(eMap mapType, HomelandsGame game)
    {
        if (mapType == eMap.Gaea)
        {
            return new MapBuilderGaea(game);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
