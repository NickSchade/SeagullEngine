using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct GameStats
{
    public Dictionary<Player, int> _numberOfStructures;
    public GameStats(HomelandsGame game)
    {
        Dictionary<Player,int> numStruts = new Dictionary<Player, int>();
        List<Player> players = game._playerSystem._players;
        foreach (Player player in players)
        {
            numStruts[player] = 0;
        }
        foreach (HomelandsLocation l in game._locations.Values)
        {
            if (l._structure != null)
            {
                numStruts[l._structure._owner] += 1;
            }
        }
        _numberOfStructures = numStruts;
    }
}
