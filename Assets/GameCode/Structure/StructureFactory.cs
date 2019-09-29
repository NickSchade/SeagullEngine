using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StructureFactory
{
    public static HomelandsStructure Make(HomelandsGame game, HomelandsLocation location, Player owner)
    {
        Debug.Log($@"Created Structure({game._gameType})");
        if (game._gameType == eGame.Exodus)
        {
            return new ExodusStructure(game, location, owner);
        }
        else if (game._gameType == eGame.Sandbox)
        {
            return new SandboxStructure(game, location, owner);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
