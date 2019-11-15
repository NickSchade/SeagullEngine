using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerSystem
{
    HomelandsGame _game;
    public Player _currentPlayer;

    public List<Player> _players;
    int _index;
    
    public PlayerSystem(HomelandsGame game, int numPlayers)
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

    public PlayerSwitchData ChangePlayer()
    {
        PlayerSwitchData psd = GetChangePlayerData();
        _index = psd._playerIndex;
        _currentPlayer = _players[_index];
        return psd;
    }

    public PlayerSwitchData GetChangePlayerData()
    {
        Player lastPlayer = _currentPlayer;
        int index = _index;
        bool isLastPlayer = false;
        if (_index == _players.Count - 1)
        {
            index = 0;
            isLastPlayer = true;
        }
        else
        {
            index++;
        }
        Player nextPlayer = _players[index];
        Debug.Log($"Changing from {lastPlayer._name} to {nextPlayer._name}");
        return new PlayerSwitchData(lastPlayer, nextPlayer, index, isLastPlayer, _players.Count);
    }

    
    public List<Player> GetPlayers()
    {
        return _players;
    }
}

public class PlayerSwitchData
{
    public Player _lastPlayer;
    public Player _nextPlayer;
    public int _playerIndex;
    public bool _isLastPlayerInQueue;
    public int _numberOfPlayers;
    public PlayerSwitchData(Player lastPlayer, Player nextPlayer, int playerIndex, bool isLastPlayer, int numberOfPlayers)
    {
        _lastPlayer = lastPlayer;
        _nextPlayer = nextPlayer;
        _playerIndex = playerIndex;
        _isLastPlayerInQueue = isLastPlayer;
        _numberOfPlayers = numberOfPlayers;
    }
}