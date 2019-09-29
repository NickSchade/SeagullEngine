using UnityEngine;
using System.Collections;

public class ViewMilitary : View
{
    public ViewMilitary(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        Color c = GetFogOfWarColor(location, stats);
        float military = stats._military._attack;
        if (military > 0f)
        {
            c = Color.Lerp(Color.white, Color.red, military / 5f);
        }

        return new LocationGraphicsData(c);
    }
}
