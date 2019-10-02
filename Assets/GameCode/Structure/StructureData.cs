using UnityEngine;
using System.Collections.Generic;

public struct StructureData
{
    float cost;
    float hitPoints;
    public Dictionary<eRadius, RadiusRange> radii;
    public StructureData(float c, float h, Dictionary<eRadius, RadiusRange> r)
    {
        cost = c;
        hitPoints = h;
        radii = r;
    }
}
