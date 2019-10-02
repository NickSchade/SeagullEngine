using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ViewGod : View
{
    public ViewGod(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        Color c = GetColorFromTerrain(location._terrain._type);

        return new LocationGraphicsData(c);
    }
}

