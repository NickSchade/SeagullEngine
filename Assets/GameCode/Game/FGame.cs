using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class FGame
{
    public static HomelandsGame Make(GameManager gameManager, GameSettings settings)
    {
        eGame gameType = settings._gameType;
        settings = GetGameSettingsForGame(settings);
        if (gameType == eGame.Exodus)
        {
            return new ExodusGame(gameManager, settings);
        }
        else if (gameType == eGame.Sandbox)
        {
            return new SandboxGame(gameManager, settings);
        }
        else
        {
            return new SandboxGame(gameManager, settings);
            //throw new System.NotImplementedException();
        }
    }

    static GameSettings GetGameSettingsForGame(GameSettings settings)
    {
        eGame gameType = settings._gameType;
        if (gameType == eGame.Sandbox)
        {
            // DO NOTHING
        }
        else if (gameType == eGame.Exodus)
        {
            settings._mapSettings._mapType = eMap.Gaea;
        }
        else if (gameType == eGame.HotSeat)
        {
            settings._tickSettings._type = eTickSystem.TurnBased;
        }
        return settings;
    }
   
}




