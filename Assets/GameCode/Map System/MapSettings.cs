using UnityEngine;
using System.Collections;


public struct MapSettings
{
    public eTileShape tileShape;
    public int xDim, yDim;
    public bool wrapEW, wrapNS;
    public MapSettings(eTileShape ts, int x, int y, bool ew, bool ns)
    {
        tileShape = ts;
        xDim = x;
        yDim = y;
        wrapEW = ew;
        wrapNS = ns;
    }

}