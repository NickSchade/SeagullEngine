using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct StatsControl
{
    public Dictionary<Player, bool> _controllingPlayers;
    public StatsControl(Dictionary<Player, bool> controllingPlayers)
    {
        _controllingPlayers = controllingPlayers;
    }
}
