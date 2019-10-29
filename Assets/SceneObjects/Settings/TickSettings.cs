using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TickSettings
{
    public eTickSystem _type;
    public float _secondsPerTurn;
    public TickSettings(TickConfigs configs)
    {
        _type = configs._type;
        _secondsPerTurn = configs._secondsPerTurn;
    }
}
