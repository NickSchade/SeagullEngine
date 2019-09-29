using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct StatsControl
{
    public List<Player> _controllingPlayers;
    public StatsControl(List<Player> controllingPlayers)
    {
        _controllingPlayers = controllingPlayers;
    }
}
