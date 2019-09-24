using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewVision : View
{
    public override LocationGraphicsData Draw(HomelandsLocation location, HomelandsPosStats stats)
    {
        Color c = GetFogOfWarColor(location, stats);

        return new LocationGraphicsData(c);
    }
}
