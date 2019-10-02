using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewExtraction : View
{
    public ViewExtraction(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        Color c = GetFogOfWarColor(location, stats);
        Dictionary<Player,float> extractions = stats._extraction._extractionRate;
        float extraction = extractions[_game._playerSystem.GetPlayer()];
        if (extraction > 0f)
        {
            c = Color.Lerp(Color.white, Color.black, extraction / 5f);
        }

        return new LocationGraphicsData(c);
    }
}
