using UnityEngine;
using System.Collections.Generic;

public class dStructurePlacement
{
    public dStructure data;
    public Player creator;
    public HomelandsLocation location;
    public dStructurePlacement(dStructure d, Player p, HomelandsLocation l)
    {
        data = d;
        creator = p;
        location = l;
    }
}