using UnityEngine;
using System.Collections.Generic;
using System;

public class TickSysTurnBased : TickSystem
{
    public TickSysTurnBased(HomelandsGame game, TickSettings settings) : base(game, settings)
    {
    }

    protected override float GetSecondsUntilNextTurn()
    {
        return float.MaxValue;
    }

    protected override float GetProgressToNextTurn(float secondsUntilNextTurn)
    {
        return 0f;
    }

}
