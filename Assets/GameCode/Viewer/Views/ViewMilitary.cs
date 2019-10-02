using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewMilitary : View
{
    public ViewMilitary(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        Color c = GetFogOfWarColor(location, stats);
        Dictionary<Player, float> militaryDict = stats._military._attack;
        float military = militaryDict[_game._playerSystem.GetPlayer()];
        if (military > 0f)
        {
            c = Color.Lerp(Color.white, Color.red, military / 5f);
        }

        return new LocationGraphicsData(c);
    }
}
