using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusGame : HomelandsGame
{
    ExodusHabitabilitySystem _habitability;

    public ExodusGame(GameManager gameManager) : base(gameManager)
    {
        Debug.Log("Constructing Exodus Game");
    }
    
}

