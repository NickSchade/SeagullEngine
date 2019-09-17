using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDrawerHomelands : ILocationDrawer
{
    HomelandsGame _game;
    public LocationDrawerHomelands()
    {
        // PLACEHOLDER
    }
    public LocationDrawerHomelands(HomelandsGame game)
    {
        _game = game;
    }
    public LocationGraphicsData Draw()
    {
        return new LocationGraphicsData(Color.red);
    }
}