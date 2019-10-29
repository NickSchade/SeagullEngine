using UnityEngine;
using System.Collections;

public class MapConfigurer : MonoBehaviour
{
    public eTileShape _tileShape;
    public eMap _mapType;
    public int _xDim;
    public int _yDim;
    public bool _wrapEW;
    public bool _wrapNS;

    public MapSettings GetMapSettings()
    {
        MapSettings data = new MapSettings(_mapType, _tileShape, _xDim, _yDim, _wrapEW, _wrapNS);
        return data;
    }
}
