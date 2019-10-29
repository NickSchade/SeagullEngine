using UnityEngine;
using System.Collections;


public static class FTickSys
{
    public static ITickSys Make(HomelandsGame game, TickSettings settings)
    {
        eTickSystem type = settings._type;
        if (type == eTickSystem.TurnBased)
        {
            return new TickSysTurnBased(game, settings);
        }
        else if (type == eTickSystem.SemiRealTime)
        {
            return new TickSysRealTime(game, settings);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}