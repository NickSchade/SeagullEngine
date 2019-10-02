using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct StatsVision
{
    public Dictionary<Player, eVisibility> _visibility;
    public StatsVision(Dictionary<Player,eVisibility> visibility)
    {
        _visibility = visibility;
    }
}
