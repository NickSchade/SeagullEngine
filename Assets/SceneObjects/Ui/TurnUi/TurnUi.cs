using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUi : MonoBehaviour
{
    public Text _text;
    public Slider _slider;

    HomelandsGame _game;
    internal void Initialize(HomelandsGame game, TickSettings tickSettings)
    {
        _game = game;
    }

    internal void UpdateUi(TickInfo tick)
    {
        _text.text = "Turn # " + tick._turnNumber.ToString();
        _slider.value = tick._progressToNextTurn;
    }
}
