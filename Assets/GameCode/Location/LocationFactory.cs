using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class LocationFactory
{
    public static HomelandsLocation Make(HomelandsGame game, Pos position, Dictionary<string,float> locationQualities)
    {
        if (game._gameType == eGame.Exodus)
        {
            return new ExodusLocation(game, position, locationQualities);
        }
        else if (game._gameType == eGame.Sandbox)
        {
            return new SandboxLocation(game, position, locationQualities);
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }
}
