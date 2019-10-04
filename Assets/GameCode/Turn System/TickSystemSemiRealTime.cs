using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TickSystemSemiRealTime : ITickSystem
{
    HomelandsGame _game;
    float _secondsPerTurn = 3f;
    DateTime _lastTurnTime;

    public TickSystemSemiRealTime(HomelandsGame game)
    {
        _game = game;

        _lastTurnTime = DateTime.Now;
        
    }
    public TickInfo GetTick(List<GraphicsData> graphicsData)
    {
        if (DateTime.Now > _lastTurnTime.AddSeconds(_secondsPerTurn))
        {
            _game.EndTurn();
            _lastTurnTime = DateTime.Now;

        }
        TickInfo tickInfo = new TickInfo(graphicsData);
        return tickInfo;
    }
}
