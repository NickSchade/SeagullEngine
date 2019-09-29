using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ePlayerSystem { TurnBased}

public interface IPlayerSystem
{
    Player GetPlayer();
}

public static class PlayerSystemFactory
{
    public static IPlayerSystem Make(ePlayerSystem type, HomelandsGame game, List<Player> players)
    {
        if (type == ePlayerSystem.TurnBased)
        {
            return new PlayerSystemTurnBased(game, players);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}

public class PlayerSystemTurnBased : IPlayerSystem
{
    HomelandsGame _game;
    Player _currentPlayer;

    List<Player> _players;
    int _index;

    public PlayerSystemTurnBased(HomelandsGame game, List<Player> players)
    {
        _game = game;
        _players = players;
        _index = 0;
        _currentPlayer = _players[_index];
        Debug.Log("Current player is " + _currentPlayer._name);
    }


    public Player GetPlayer()
    {
        return _currentPlayer;
    }
}
