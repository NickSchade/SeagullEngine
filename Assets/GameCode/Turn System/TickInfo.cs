using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickInfo
{
    public List<GraphicsData> _graphicsData;
    public int _turnNumber;
    public float _secondsUntilNextTurn;
    public float _progressToNextTurn;
    public TickInfo(List<GraphicsData> graphicsData, int turnNumber, float timeUntilNextTurn, float percentToNextTurn)
    {
        _graphicsData = graphicsData;
        _turnNumber = turnNumber;
        _secondsUntilNextTurn = timeUntilNextTurn;
        _progressToNextTurn = percentToNextTurn;
    }

}
