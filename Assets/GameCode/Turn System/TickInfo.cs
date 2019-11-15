using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickInfo
{
    public List<GraphicsData> _graphicsData;
    public HomelandsTurnData _turnData;
    public PlayerSwitchData _switchData;

    public int _turnNumber;
    public float _secondsUntilNextTurn;
    public float _progressToNextTurn;


    public TickInfo(List<GraphicsData> graphicsData, HomelandsTurnData turnData, int turnNumber, float timeUntilNextTurn, float percentToNextTurn)
    {
        _turnData = turnData;
        _graphicsData = graphicsData;
        //_switchData = switchData;
        _turnNumber = turnNumber;
        _secondsUntilNextTurn = timeUntilNextTurn;
        _progressToNextTurn = percentToNextTurn;
    }

}
