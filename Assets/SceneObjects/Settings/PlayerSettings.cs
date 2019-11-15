using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public int _numberOfPlayers;
    public PlayerSettings(PlayerConfigs configs)
    {
        _numberOfPlayers = configs._numberOfPlayers;
    }
    public PlayerSettings(int numberOfPlayers)
    {
        _numberOfPlayers = numberOfPlayers;
    }
    public static PlayerSettings ExodusSettings()
    {
        int numberOfPlayer = 1;

        return new PlayerSettings(numberOfPlayer);
    }
}
