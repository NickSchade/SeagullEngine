using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View
{
    public abstract LocationGraphicsData Draw(HomelandsLocation location, HomelandsPosStats stats);
    protected Color GetColorFromTerrain(eTerrain terrain)
    {
        Color c = Color.magenta;
        if (terrain == eTerrain.Land)
        {
            c = Color.green;
        }
        else if (terrain == eTerrain.Mountain)
        {
            c = Color.yellow;
        }
        else if (terrain == eTerrain.Sea)
        {
            c = Color.blue;
        }
        else
        {
            throw new System.NotImplementedException();
        }
        return c;
    }
    protected Color GetFogOfWarColor(HomelandsLocation location, HomelandsPosStats stats)
    {
        Color c = Color.magenta;
        eVisibility visibility = stats._vision._visibility;
        if (visibility == eVisibility.Visible)
        {
            c = GetColorFromTerrain(location._terrain);
        }
        else if (visibility == eVisibility.Fog)
        {
            c = Color.black;
        }
        else if (visibility == eVisibility.Unexplored)
        {
            c = Color.black;
        }
        else
        {
            throw new System.NotImplementedException();
        }
        return c;
    }
}