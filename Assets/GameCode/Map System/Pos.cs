using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pos
{
    public Loc gridLoc;
    public Loc mapLoc;
    public List<Pos> neighbors;

    public Pos(Loc _loc, IMapLocSetter mapLocSetter)
    {
        gridLoc = _loc;
        mapLoc = mapLocSetter.GetMapLoc(this);
    }
    
}



