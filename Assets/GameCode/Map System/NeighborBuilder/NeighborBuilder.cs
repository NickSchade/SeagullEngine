using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NeighborBuilder
{
    public abstract void SetNeighbors(Pos p);
}

public static class NeighborBuilderFactory
{
    public static NeighborBuilder Make(MapInfo mapInfo)
    {
        if (mapInfo.tileShape == eTileShape.Square)
        {
            return new NeighborBuilderSquare(mapInfo);
        }
        else if (mapInfo.tileShape == eTileShape.Hex)
        {
            return new NeighborBuilderHex(mapInfo);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
