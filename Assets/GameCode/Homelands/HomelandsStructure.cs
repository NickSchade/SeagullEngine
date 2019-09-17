using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomelandsStructure : IStructure
{
    public Dictionary<eRadius, RadiusRange> _radii;
    public HomelandsStructureDeath _deathSystem;
    public Dictionary<eCost, CostAmount> _cost;
    void Initialize(eGame gameType)
    {
        if (gameType == eGame.Exodus)
        {
            _deathSystem = new ExodusStructureDeath();
        }
        else if (gameType == eGame.Sandbox)
        {
            _deathSystem = new HomelandsSandboxStructureDeath();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
    public void Click()
    {
        throw new System.NotImplementedException();
    }
    public StructureGraphicsData Draw()
    {
        return new StructureGraphicsData(Color.white);
    }

}
