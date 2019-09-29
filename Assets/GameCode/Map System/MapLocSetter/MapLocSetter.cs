using UnityEngine;
using System.Collections;


public interface IMapLocSetter
{
    Loc GetMapLoc(Pos p);
}
public static class MapLocSetterFactory
{
    public static IMapLocSetter Make(eTileShape tileShape)
    {
        if (tileShape == eTileShape.Square)
        {
            return new MapLocSetterSquare();
        }
        else if (tileShape == eTileShape.Hex)
        {
            return new MapLocSetterHex();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}