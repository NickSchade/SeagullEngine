using UnityEngine;
using System.Collections;


public static class FPlayerSys
{
    public static IPlayerSys Make(ePlayerSys type, HomelandsGame game, int numPlayers)
    {
        if (type == ePlayerSys.Serial)
        {
            return new PlayerSysSerial(game, numPlayers);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
    public static IPlayerSys Make(HomelandsGame game, PlayerSettings settings)
    {
        ePlayerSys type = settings._playerSystem;

        if (type == ePlayerSys.Serial)
        {
            return new PlayerSysSerial(game, settings._numberOfPlayers);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}