using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSwitchScreenUi : MonoBehaviour
{
    public Text _lastPlayer;
    public Text _to;
    public Text _nextPlayer;

    HomelandsGame _game;

    public void InitializeUi(HomelandsGame game)
    {
        _game = game;
    }

    public void UpdateUi(PlayerSwitchData data)
    {
        _lastPlayer.text = data._lastPlayer._name.ToString();
        _lastPlayer.color = data._lastPlayer._color;

        _nextPlayer.text = data._nextPlayer._name.ToString();
        _nextPlayer.color = data._nextPlayer._color;
    }
}
