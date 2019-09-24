using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GameFactory
{
    public static HomelandsGame Make(GameManager gameManager)
    {
        eGame gameType = gameManager._gameType;
        if (gameType == eGame.Exodus)
        {
            return new ExodusGame(gameManager);
        }
        else if (gameType == eGame.Sandbox)
        {
            return new SandboxGame(gameManager);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
   
}




