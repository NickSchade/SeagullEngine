using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SandboxGame : HomelandsGame
{
    public SandboxGame(GameManager gameManager, GameSettings settings) : base(gameManager, settings)
    {
        Debug.Log("Constructing Homelands Sandbox Game");
    }
}
