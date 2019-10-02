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
    public MapInfo(HomelandsGame g, Dictionary<Pos,HomelandsLocation> locs, MapSettings settings, Dictionary<string,Pos> pm)
    {

        game = g;
        locations = locs;
        tileShape = settings.tileShape;
        xDim = settings.xDim;
        yDim = settings.yDim;
        wrapEW = settings.wrapEW;
        wrapNS = settings.wrapNS;
        pathMap = pm;
    }
}
