using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapSettings
{
    public eMap _mapType;
    public eTileShape _tileShape;
    public int _xDim;
    public int _yDim;
    public bool _wrapEastWest;
    public bool _wrapNorthSouth;
    public float _percentOcean;
    public float _percentRiver;
    public int _expansionFactor;
    public WaterBodyPrefence _waterBodyPref;


    public MapSettings(eMap mapType,
                        eTileShape tileShape,
                        int xDim,
                        int yDim,
                        bool wrapEastWest,
                        bool wrapNorthSouth,
                        float percentOcean,
                        float percentRiver,
                        int expansionFactor,
                        WaterBodyPrefence waterBodyPref)
    {
        _mapType = mapType;
        _tileShape = tileShape;
        _xDim = xDim;
        _yDim = yDim;
        _wrapEastWest = wrapEastWest;
        _wrapNorthSouth = wrapNorthSouth;
        _percentOcean = percentOcean;
        _percentRiver = percentRiver;
        _expansionFactor = expansionFactor;
        _waterBodyPref = waterBodyPref;
    }

    public MapSettings(MapConfigs configs)
    {
        _mapType = configs._mapType;
        _tileShape = configs._tileShape;
        _xDim = configs.xDim;
        _yDim = configs.yDim;
        _wrapEastWest = configs.wrapEastWest;
        _wrapNorthSouth = configs.wrapNorthSouth;
        _percentOcean = configs.percentLand;
        _percentRiver = configs.percentRiver;
        _expansionFactor = configs.expansionFactor;
        _waterBodyPref = configs.waterBodyPref;
    }

    public static MapSettings HotseatSettings()
    {
        eMap mapType = eMap.Basic;
        eTileShape tileShape = eTileShape.Square;
        int xDim = 19;
        int yDim = 19;
        bool wrapEastWest = false;
        bool wrapNorthSouth = false;
        float percentOcean = 0.3f;
        float percentRiver = 0.01f;
        int expansionFactor = 0;
        WaterBodyPrefence wbp = WaterBodyPrefence.Continent;

        MapSettings settings = new MapSettings(mapType,
                                                tileShape,
                                                xDim,
                                                yDim,
                                                wrapEastWest,
                                                wrapNorthSouth,
                                                percentOcean,
                                                percentRiver,
                                                expansionFactor,
                                                wbp);
        return settings;
    }
}
