using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ePlayerSys { Serial}

public interface IPlayerSys
{
    Player GetPlayer();
    List<Player> GetPlayers();
    void AddPlayer();
    void ChangePlayer();
}

