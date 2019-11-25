using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pos
{
    public Loc _gridLoc;
    public Loc _mapLoc;
    public List<Pos> _neighbors;

    public Pos(Loc _loc, IMapLocSetter mapLocSetter)
    {
        _gridLoc = _loc;
        _mapLoc = mapLocSetter.GetMapLoc(this);
    }
    public Pos(Loc gridLoc, Loc mapLoc)
    {
        _gridLoc = gridLoc;
        _mapLoc = mapLoc;
    }
    
}



