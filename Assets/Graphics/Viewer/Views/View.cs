using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View
{
    public HomelandsGame _game;
    public View(HomelandsGame game)
    {
        Initialize(game);
    }
    public void Initialize(HomelandsGame game)
    {
        _game = game;
        //Debug.Log("View Initialized");
    }
    public abstract LocationGraphicsData Draw(HomelandsLocation location, Stats stats);
    protected Color GetColorFromTerrain(eTerrain terrain)
    {
        Color c = Color.magenta;
        if (terrain == eTerrain.Land)
        {
            c = Color.green;
        }
        else if (terrain == eTerrain.Mountain)
        {
            c = Color.white;
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
    protected Color GetFogOfWarColor(HomelandsLocation location, Stats stats)
    {
        Color c = Color.magenta;
        eVisibility visibility = stats._vision._visibility[_game._playerSystem._currentPlayer];
        if (visibility == eVisibility.Visible)
        {
            c = GetColorFromTerrain(location._terrain._type);
        }
        else if (visibility == eVisibility.Fog)
        {
            c = Color.Lerp(GetColorFromTerrain(location._terrain._type), Color.black, 0.5f);
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