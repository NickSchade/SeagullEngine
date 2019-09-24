using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusGame : HomelandsGame
{
    ExodusPlayerResources _resources;
    ExodusHabitabilitySystem _habitability;

    public ExodusGame(GameManager gameManager) : base(gameManager)
    {
        Debug.Log("Constructing Exodus Game");
    }

    void ClickLocationWithStructure(Pos p)
    {

    }
    void ClickLocationWithoutStructure(Pos p)
    {

    }
}

