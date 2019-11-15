using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct MapInfo
{
    public HomelandsGame game;
    public Dictionary<Pos, HomelandsLocation> locations;
    public eTileShape tileShape;
    public bool wrapEW, wrapNS;
    public MapSettings _settings;
    public Dictionary<string, Pos> pathMap;
    public MapInfo(HomelandsGame g, Dictionary<Pos,HomelandsLocation> locs, MapSettings settings, Dictionary<string,Pos> pm)
    {
        _settings = settings;
        game = g;
        locations = locs;
        tileShape = settings._tileShape;
        wrapEW = settings._wrapEastWest;
        wrapNS = settings._wrapNorthSouth;
        pathMap = pm;
    }
}
