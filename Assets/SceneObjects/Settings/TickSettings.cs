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
    public TickSettings(eTickSystem type,
                        float secondsPerTurn)
    {
        _type = type;
        _secondsPerTurn = secondsPerTurn;
    }
    public static TickSettings ExodusSettings()
    {
        eTickSystem type = eTickSystem.SemiRealTime;
        float secondsPerTurn = 30f;

        return new TickSettings(type, 
                                secondsPerTurn);
    }
    public static TickSettings HotseatSettings()
    {
        eTickSystem type = eTickSystem.TurnBased;
        float secondsPerTurn = 1f;

        return new TickSettings(type, 
                                secondsPerTurn);
    }
}
