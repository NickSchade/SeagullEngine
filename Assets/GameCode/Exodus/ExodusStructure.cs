using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusStructure : HomelandsStructure, IStructure
{
    public List<ExodusStructureAbility> _abilities;
    void Initialize()
    {
        _deathSystem = new ExodusStructureDeath();
    }
}

public class ExodusStructureAbility
{

}