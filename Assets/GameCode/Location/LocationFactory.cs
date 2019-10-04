using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LocationFactory
{
    public static HomelandsLocation Make(HomelandsGame game, Pos position, HomelandsTerrain terrain, HomelandsResource resource)
    {
        if (game._gameType == eGame.Exodus)
        {
            return new ExodusLocation(game, position, terrain, resource);
        }
        else if (game._gameType == eGame.Sandbox)
        {
            return new SandboxLocation(game, position, terrain, resource);
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }
}
