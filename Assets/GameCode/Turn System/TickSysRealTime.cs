using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TickSysRealTime : BTickSys, ITickSys
{
    float _secondsPerTurn;

    public TickSysRealTime(HomelandsGame game, TickSettings settings) : base(game, settings)
    {
        _secondsPerTurn = settings._secondsPerTurn;
    }

    protected override float GetSecondsUntilNextTurn()
    {
        float secondsUntilNextTurn = (float)(_lastTurnTime.AddSeconds(_secondsPerTurn) - DateTime.Now).TotalSeconds;
        return secondsUntilNextTurn;
    }

    protected override float GetProgressToNextTurn(float secondsUntilNextTurn)
    {
        float percentUntilNextTurn = ((_secondsPerTurn - secondsUntilNextTurn))/_secondsPerTurn;
        return percentUntilNextTurn;
    }
}
