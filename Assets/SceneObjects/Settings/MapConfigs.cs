using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfigs : MonoBehaviour
{
    public eMap _mapType;
    public eTileShape _tileShape;
    public int xDim;
    public int yDim;
    public bool wrapEastWest;
    public bool wrapNorthSouth;
    [Range(0.0f,1.0f)] public float percentLand;
    [Range(0.0f, 0.4f)] public float percentRiver;
    public int expansionFactor;
    public WaterBodyPrefence waterBodyPref;

}
