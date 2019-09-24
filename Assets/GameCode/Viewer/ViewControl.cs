using UnityEngine;
using System.Collections;

public class ViewControl : ViewVision
{
    public override LocationGraphicsData Draw(HomelandsLocation location, HomelandsPosStats stats)
    {
        Color c = GetFogOfWarColor(location, stats);
        c = stats._control._controlled ? Color.white : c;
        return new LocationGraphicsData(c);
    }

}
