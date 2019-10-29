using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;

public abstract class BTickSys 
{
    public HomelandsGame _game;

    public int _tickNumber;
    public int _turnNumber;
    public DateTime _lastTurnTime;

    public BTickSys(HomelandsGame game, TickSettings settings)
    {
        _game = game;
        _tickNumber = 0;
        _turnNumber = 0;
        _lastTurnTime = DateTime.Now;
    }
    protected abstract float GetSecondsUntilNextTurn();
    protected abstract float GetProgressToNextTurn(float secondsUntilNextTurn);
    protected void UpdateInternal(HomelandsTurnData htd)
    {
        _tickNumber++;
        if (htd._endTurn)
        {
            _turnNumber++;
            _lastTurnTime = DateTime.Now;

            _game.EndTurn();
        }
    }


    public TickInfo GetTick(List<GraphicsData> graphicsData, HomelandsTurnData turnData)
    {
        TickInfo tickInfo = MakeTick(graphicsData);

        if (tickInfo._secondsUntilNextTurn < 0f)
            turnData._endTurn = true;

        UpdateInternal(turnData);

        return tickInfo;
    }
    protected TickInfo MakeTick(List<GraphicsData> graphicsData)
    {
        float secondsUntilNextTurn = GetSecondsUntilNextTurn();
        float percentUntilNextTurn = GetProgressToNextTurn(secondsUntilNextTurn);
        TickInfo tickInfo = new TickInfo(graphicsData, _turnNumber, secondsUntilNextTurn, percentUntilNextTurn);
        return tickInfo;
    }
}

