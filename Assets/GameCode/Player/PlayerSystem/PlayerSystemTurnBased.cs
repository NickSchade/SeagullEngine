using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class PlayerSystemTurnBased : IPlayerSystem
{
    HomelandsGame _game;
    Player _currentPlayer;

    List<Player> _players;
    int _index;
    

    public PlayerSystemTurnBased(HomelandsGame game)
    {
        _game = game;
        _players = new List<Player>();
        AddPlayer();
        _index = 0;
        _currentPlayer = _players[_index];


    }

    public void AddPlayer()
    {
        List<string> playerNames = new List<string> { "P1", "P2" };
        List<Color> playerColors = new List<Color> { Color.red, Color.blue };
        int playerNumber = _players.Count;
        string name = playerNames[playerNumber];
        Color color = playerColors[playerNumber];
        Player newPlayer = new Player(name, color);
        _players.Add(newPlayer);
        Debug.Log($@"Added player named {name} with color {color.ToString()}");
    }

    public void ChangePlayer()
    {
        string oldPlayer = _currentPlayer._name.ToString();
        _index = _index == _players.Count - 1 ? 0 : _index+1;
        Debug.Log($"index is {_index} of {_players.Count}");
        _currentPlayer = _players[_index];
        Debug.Log($@"Changed player from {oldPlayer} to {_currentPlayer._name}");
    }


    public Player GetPlayer()
    {
        return _currentPlayer;
    }
    public List<Player> GetPlayers()
    {
        return _players;
    }
}