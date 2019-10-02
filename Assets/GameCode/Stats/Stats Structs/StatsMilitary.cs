using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct StatsMilitary
{
    public Dictionary<Player, float> _attack;
    public StatsMilitary(Dictionary<Player, float> attack)
    {
        _attack = attack;
    }
}