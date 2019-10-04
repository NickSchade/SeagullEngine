using UnityEngine;
using System.Collections.Generic;

public class StructureData
{
    public float cost;
    public float hitPoints;
    public Dictionary<eRadius, RadiusRange> radii;
    public StructureData(float c, float h, Dictionary<eRadius, RadiusRange> r)
    {
        cost = c;
        hitPoints = h;
        radii = r;
    }
}

public class StructurePlacementData
{
    public StructureData data;
    public Player creator;
    public HomelandsLocation location;
    public StructurePlacementData(StructureData d, Player p, HomelandsLocation l)
    {
        data = d;
        creator = p;
        location = l;
    }
}
