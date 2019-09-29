using UnityEngine;
using System.Collections;


public struct StatsExtraction
{
    public float _extractionRate; // replace with HomelandsExtractionRate object or even D<eResource,HomelandsExtractionRate> after those become things
    public StatsExtraction(float extractionRate)
    {
        _extractionRate = extractionRate;
    }
}