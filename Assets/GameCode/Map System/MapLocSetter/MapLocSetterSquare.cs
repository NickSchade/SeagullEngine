using UnityEngine;
using System.Collections;

public class MapLocSetterSquare : IMapLocSetter
{
    public Loc GetMapLoc(Pos p)
    {
        Loc mapLoc = new Loc(p._gridLoc.coordinates);
        return mapLoc;
    }
}