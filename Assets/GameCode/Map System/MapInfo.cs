using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct MapInfo
{
    public HomelandsGame game;
    public Dictionary<Pos, HomelandsLocation> locations;
    public eTileShape tileShape;
    public int xDim, yDim;
    public bool wrapEW, wrapNS;
    public Dictionary<string, Pos> pathMap;
    public MapInfo(HomelandsGame g, Dictionary<Pos,HomelandsLocation> locs, eTileShape ts, int x, int y, bool ew, bool ns, Dictionary<string,Pos> pm)
    {
        game = g;
        locations = locs;
        tileShape = ts;
        xDim = x;
        yDim = y;
        wrapEW = ew;
        wrapNS = ns;
        pathMap = pm;
    }
}