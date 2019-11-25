using UnityEngine;
using System.Collections;

public class MapLocSetterHex : IMapLocSetter
{
    public Loc GetMapLoc(Pos p)
    {
        float x = Mathf.Sqrt(3) * (p._gridLoc.x() - 0.5f * (p._gridLoc.y() % 2f)) / 1.9f;
        float y = (3 / 2) * p._gridLoc.y() / 1.3f;
        Loc mapLoc = new Loc(x, y);
        return mapLoc;
    }
}