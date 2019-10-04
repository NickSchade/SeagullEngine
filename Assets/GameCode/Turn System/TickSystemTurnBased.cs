using UnityEngine;
using System.Collections.Generic;

public class TickSystemTurnBased : ITickSystem
{
    HomelandsGame _game;
    int _turnNumber;
    public TickSystemTurnBased(HomelandsGame game)
    {
        _game = game;
        _turnNumber = 0;
    }
    public TickInfo GetTick(List<GraphicsData> graphicsData)
    {
        _turnNumber++;
        TickInfo tickInfo = new TickInfo(graphicsData);
        return tickInfo;
    }
}
