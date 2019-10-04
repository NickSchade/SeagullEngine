using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class PlayerSystemTurnBased : IPlayerSystem
{
    HomelandsGame _game;
    Player _currentPlayer;

    List<Player> _players;
    int _index;
    

    public PlayerSystemTurnBased(HomelandsGame game, int numPlayers)
    {
        _game = game;
        _players = new List<Player>();
        AddPlayers(numPlayers);
        _index = 0;
        _currentPlayer = _players[_index];


    }

    PlayerDemographics GetStartingPlayer()
    {

        List<string> playerNames = new List<string> { "P1", "P2", "P3" };
        List<Color> playerColors = new List<Color> { Color.red, Color.blue, Color.cyan };
        int playerNumber = _players.Count;
        string name = playerNames[playerNumber];
        Color color = playerColors[playerNumber];
        PlayerDemographics demo = new PlayerDemographics(name, color, 3f);
        return demo;
    }
    void AddPlayers(int numPlayers)
    {
        for (int i = 0; i < numPlayers; i++)
        {
            AddPlayer();
        }
    }

    public void AddPlayer()
    {
        PlayerDemographics demo = GetStartingPlayer();
        Player newPlayer = new Player(_game, demo);
        _players.Add(newPlayer);
        Debug.Log($@"Added player named {demo.name} with color {demo.color.ToString()}");
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