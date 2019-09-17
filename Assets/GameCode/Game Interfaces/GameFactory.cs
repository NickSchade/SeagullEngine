using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGame { Exodus, HomelandsSandbox}

public static class GameFactory
{
    public static HomelandsGame Make(GameManager gameManager)
    {
        eGame gameType = gameManager._gameType;
        if (gameType == eGame.Exodus)
        {
            return new ExodusGame(gameManager);
        }
        else if (gameType == eGame.HomelandsSandbox)
        {
            return new HomelandsSandboxGame(gameManager);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
   
}

public enum eTileShape { SQUARE, HEX };
public enum eVisibility { Visible, Fog, Unexplored };
public enum eInput { Down, Held, Up}



