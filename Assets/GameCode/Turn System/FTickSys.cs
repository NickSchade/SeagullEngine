using UnityEngine;
using System.Collections;


public static class FTickSystem
{
    public static TickSystem Make(HomelandsGame game, TickSettings settings)
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