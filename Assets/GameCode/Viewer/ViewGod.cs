using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ViewGod : View
{
    public override LocationGraphicsData Draw(HomelandsLocation location, HomelandsPosStats stats)
    {
        Color c = GetColorFromTerrain(location._terrain);

        return new LocationGraphicsData(c);
    }
}

