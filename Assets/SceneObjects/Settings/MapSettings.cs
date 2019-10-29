using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapSettings
{
    public eMap _mapType;
    public eTileShape _tileShape;
    public int xDim;
    public int yDim;
    public bool wrapEastWest;
    public bool wrapNorthSouth;
    
    public MapSettings(MapConfigs configs)
    {
        _mapType = configs._mapType;
        _tileShape = configs._tileShape;
        xDim = configs.xDim;
        yDim = configs.yDim;
        wrapEastWest = configs.wrapEastWest;
        wrapNorthSouth = configs.wrapNorthSouth;
    }
}
