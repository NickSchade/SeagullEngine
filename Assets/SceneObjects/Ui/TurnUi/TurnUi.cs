using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUi : MonoBehaviour
{
    public TurnUiTurnbased _turnBased;
    public TurnUiRealtime _realTime;

    eTickSystem _tickSystem;

    public void Initialize(HomelandsGame _game, TickSettings settings)
    {
        _tickSystem = settings._type;

        if (_tickSystem == eTickSystem.SemiRealTime)
        {
            _realTime.InitializeUi(_game);
        }
        else if (_tickSystem == eTickSystem.TurnBased)
        {
            _turnBased.InitializeUi(_game);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
    public void UpdateUi(TickInfo tick)
    {
        if (_tickSystem == eTickSystem.SemiRealTime)
        {
            _realTime.UpdateUi(tick);
        }
        else if (_tickSystem == eTickSystem.TurnBased)
        {
            _turnBased.UpdateUi(tick);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
