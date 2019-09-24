using UnityEngine;
using System.Collections;

public class ViewExtraction : View
{
    public override LocationGraphicsData Draw(HomelandsLocation location, HomelandsPosStats stats)
    {
        Color c = GetFogOfWarColor(location, stats);
        float extraction = stats._extraction._extractionRate;
        if (extraction > 0f)
        {
            c = Color.Lerp(Color.white, Color.black, extraction / 5f);
        }

        return new LocationGraphicsData(c);
    }
}
