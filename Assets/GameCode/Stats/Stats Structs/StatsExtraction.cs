using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct StatsExtraction
{
    public Dictionary<Player, float> _extractionRate;
    public StatsExtraction(Dictionary<Player, float> extractionRate)
    {
        _extractionRate = extractionRate;
    }
}