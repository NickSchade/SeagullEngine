using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StructureFactory
{
    public static HomelandsStructure Make(HomelandsGame game, HomelandsLocation location)
    {
        Debug.Log($@"Created Structure({game._gameType})");
        if (game._gameType == eGame.Exodus)
        {
            return new ExodusStructure(game, location);
        }
        else if (game._gameType == eGame.Sandbox)
        {
            return new SandboxStructure(game, location);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
