using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusStructure : HomelandsStructure
{
    public List<ExodusStructureAbility> _abilities;

    public ExodusStructure(HomelandsGame game, StructurePlacementData data) : base(game, data)
    {
    }
}
