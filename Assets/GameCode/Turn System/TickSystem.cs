using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;

public abstract class TickSystem 
{
    public HomelandsGame _game;

    public int _tickNumber;
    public int _turnNumber;
    public float _lastTurnTime;

    public TickSystem(HomelandsGame game, TickSettings settings)
    {
        _game = game;
        _tickNumber = 0;
        _turnNumber = 0;
        _lastTurnTime = Time.time;
    }
    protected abstract float GetSecondsUntilNextTurn();
    protected abstract float GetProgressToNextTurn(float secondsUntilNextTurn);
    protected void UpdateInternal(TickInfo tick)
    {
        _tickNumber++;
        if (tick._turnData._kho._keyActions[ePlayerAction.EndTurn])
        {
            if (_game._gameManager._hudManager.ScreenIsActive())
            {
                _game._gameManager._hudManager.ToggleScreen();
                TogglePause();
            }
            else
            {
                PlayerSwitchData psd = _game._playerSystem.ChangePlayer();
                if (_game._playerSystem._players.Count > 1 && !_game._gameManager._hudManager.ScreenIsActive())
                {
                    _game._gameManager._hudManager.ToggleScreen(psd);
                    TogglePause();
                }

                if (psd._isLastPlayerInQueue)
                {
                    _game.EndTurn();
                }
            }
        }

        if (tick._turnData._kho._keyActions[ePlayerAction.ChangeView])
            _game._viewer.ToggleNextView();
        
        if (tick._turnData._kho._keyActions[ePlayerAction.Help])
            _game._gameManager._hudManager.ToggleHelp();

        if (tick._turnData._kho._keyActions[ePlayerAction.Pause])
            TogglePause();
            

    }


    public TickInfo GetTick(List<GraphicsData> graphicsData, HomelandsTurnData turnData)
    {
        TickInfo tickInfo = MakeTick(graphicsData, turnData);

        if (tickInfo._secondsUntilNextTurn < 0f)
            turnData._kho._keyActions[ePlayerAction.EndTurn] = true;

        UpdateInternal(tickInfo);

        CheckEndCondition();

        return tickInfo;
    }
    void CheckEndCondition()
    {
        _game.RemoveDeadPlayers();

        _game.CheckEndConditions();
    }


    protected TickInfo MakeTick(List<GraphicsData> graphicsData, HomelandsTurnData turnData)
    {
        float secondsUntilNextTurn = GetSecondsUntilNextTurn();
        float percentUntilNextTurn = GetProgressToNextTurn(secondsUntilNextTurn);
        TickInfo tickInfo = new TickInfo(graphicsData, turnData, _turnNumber, secondsUntilNextTurn, percentUntilNextTurn);
        return tickInfo;
    }

    public void TogglePause()
    {
        float scale = Time.timeScale;
        Time.timeScale = 1f - scale;
    }
}

