using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EndSystem
{
    protected HomelandsGame _game;
    public EndSystem(HomelandsGame game)
    {
        _game = game;
    }
    public abstract void CheckEndConditions();
    public void EndGame()
    {
        //Application.Quit();
        //_game._tickSystem.TogglePause();
        _game._gameManager.QuitGame();
    }
}

public class FEndSystem
{
    public static EndSystem Make(eEndCondition condition, HomelandsGame game)
    {
        if (condition == eEndCondition.Survival)
        {
            return new EndSystemSurvival(game);
        }
        else if (condition == eEndCondition.LastOneStanding)
        {
            return new EndSystemLastOneStanding(game);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}

public class EndSystemSurvival : EndSystem
{
    public EndSystemSurvival(HomelandsGame game) : base(game) {}

    public override void CheckEndConditions()
    {
        if (_game._playerSystem._players.Count == 0)
        {
            _game._playerSystem._currentPlayer = null;
            Debug.Log("Game Over - Your Survival has Ended");
            EndGame();
        }
    }
}


public class EndSystemLastOneStanding : EndSystem
{
    public EndSystemLastOneStanding(HomelandsGame game) : base(game) {}

    public override void CheckEndConditions()
    {
        if (_game._playerSystem._players.Count == 1)
        {
            Debug.Log($"Game Over - {_game._playerSystem._players[0]._name} is the Last One Standing");
            EndGame();
        }
    }
}
