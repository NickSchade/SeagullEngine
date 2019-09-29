using UnityEngine;
using System.Collections;

public struct GameStats
{
    public int _numberOfStructures;
    public GameStats(HomelandsGame game)
    {
        int numStruts = 0;
        foreach (HomelandsLocation l in game._locations.Values)
        {
            if (l._structure != null)
            {
                numStruts++;
            }
        }
        _numberOfStructures = numStruts;
    }
}
