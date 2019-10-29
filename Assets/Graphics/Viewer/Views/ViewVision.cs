using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewVision : View
{
    public ViewVision(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        Color c = GetFogOfWarColor(location, stats);

        return new LocationGraphicsData(c);
    }
}
