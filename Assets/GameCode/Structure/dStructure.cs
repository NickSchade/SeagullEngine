using UnityEngine;
using System.Collections.Generic;

public class dStructure
{
    public float cost;
    public float hitPoints;
    public Dictionary<eRadius, RadiusRange> radii;
    public dStructure(float c, float h, Dictionary<eRadius, RadiusRange> r)
    {
        cost = c;
        hitPoints = h;
        radii = r;
    }
}

