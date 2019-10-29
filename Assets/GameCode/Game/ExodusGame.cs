using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusGame : HomelandsGame
{
    ExodusHabitabilitySystem _habitability;

    public ExodusGame(GameManager gameManager, GameSettings settings) : base(gameManager, settings)
    {
        Debug.Log("Constructing Exodus Game");
    }
}

