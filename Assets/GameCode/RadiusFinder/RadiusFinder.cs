using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public static class RadiusFinder
{
    public static List<HomelandsLocation> GetLocationsInRadius(HomelandsLocation location, RadiusRange radiusRange)
    {
        List<Pos> posInRadius = Pathfinder.GetPosInRadius(location._game._locations.Keys.ToList(), location._pos, radiusRange);
        List<HomelandsLocation> locations = new List<HomelandsLocation>();
        foreach (Pos p in posInRadius)
        {
            locations.Add(location._game._locations[p]);
        }
        return locations;
        
    }

}
