using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StructureFactory
{
    public static IStructure Make(eGame gameType)
    {
        Debug.Log($@"Created Structure({gameType})");
        if (gameType == eGame.Exodus)
        {
            return new ExodusStructure();
        }
        else if (gameType == eGame.Sandbox)
        {
            return new HomelandsStructure();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}

public interface IStructure
{
    void Click();
    StructureGraphicsData Draw();
}
