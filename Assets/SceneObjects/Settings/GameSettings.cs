using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameSettings 
{
    public eGame _gameType;
    
    public MapSettings _mapSettings;
    
    public PlayerSettings _playerSettings;

    public TickSettings _tickSettings;

    public GameSettings(eGame gameType, MapSettings map, PlayerSettings player,  TickSettings tick)
    {
        _gameType = gameType;

        _mapSettings = map;

        _playerSettings = player;

        _tickSettings = tick;
    }
    public GameSettings(GameConfigs configs)
    {
        _gameType = configs._gameType;
        
        _mapSettings = new MapSettings(configs._mapConfigs);

        _playerSettings = new PlayerSettings(configs._playerConfigs);

        _tickSettings = new TickSettings(configs._tickConfigs);
    }
}

public class FGameSettings
{
    public static GameSettings Make(GameConfigs configs)
    {
        eGame gameType = configs._gameType;
        if (gameType == eGame.Sandbox)
        {
            return new GameSettings(configs);
        }
        else if (gameType == eGame.Exodus)
        {
            return new GameSettings(eGame.Exodus, new MapSettings(configs._mapConfigs), new PlayerSettings(configs._playerConfigs), new TickSettings(configs._tickConfigs));
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}

