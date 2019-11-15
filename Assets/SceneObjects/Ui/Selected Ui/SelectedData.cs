using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedData 
{
    public Pos _pos;
    public eTerrain _terrain;
    public HomelandsStructure _structure;
    public SelectedData(HomelandsLocation location)
    {
        _pos = location._pos;
        _terrain = location._terrain._type;
        _structure = location._structure;
    }
}
