using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LocationFactory
{
    public static HomelandsLocation Make(HomelandsGame game, Pos position, HomelandsTerrain terrain, HomelandsResource resource)
    {
        eGame gameType = game._settings._gameType;
        if (gameType == eGame.Exodus)
        {
            return new ExodusLocation(game, position, terrain, resource);
        }
        else if (gameType == eGame.Sandbox)
        {
            return new SandboxLocation(game, position, terrain, resource);
        }
        else
        {
            return new SandboxLocation(game, position, terrain, resource);
            //throw new KeyNotFoundException();
        }
    }
}
