using UnityEngine;
using System.Collections;

public class HomelandsTurnData 
{
    public dStructurePlacement _structureToBuild;
    public bool _endTurn;
    public HomelandsTurnData(dStructurePlacement structureToBuild, bool endturn)
    {
        _structureToBuild = structureToBuild;
        _endTurn = endturn;
    }
}
