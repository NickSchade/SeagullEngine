using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public int _numberOfPlayers;
    public ePlayerSys _playerSystem;
    public eBuildQueue _buildQueueType;
    public PlayerSettings(int numberOfPlayers, ePlayerSys playerSys, eBuildQueue buildQueueType)
    {
        _numberOfPlayers = numberOfPlayers;
        _playerSystem = playerSys;
        _buildQueueType = buildQueueType;
    }
    public PlayerSettings(PlayerConfigs configs)
    {
        _numberOfPlayers = configs._numberOfPlayers;
        _playerSystem = configs._playerSystem;
        _buildQueueType = configs._buildQueueType;
    }
}
