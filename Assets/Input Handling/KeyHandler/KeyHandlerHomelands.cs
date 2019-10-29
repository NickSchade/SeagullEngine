using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyHandlerHomelands : IKeyHandler
{
    HomelandsGame _game;
    KeyCode _changePlayers = KeyCode.P;
    KeyCode _changeView = KeyCode.V;
    KeyCode _endTurn = KeyCode.Return;

    public KeyHandlerHomelands(HomelandsGame game)
    {
        _game = game;
    }
    public KeyHandlerOutput HandleKeys(KeyHandlerInfo keyHandlerInfo)
    {
        Dictionary<KeyCode, KeyCodeInfo> keys = keyHandlerInfo._keysInterfaced;

        bool changedView = ChangeView(keys);
        bool changedPlayer = ChangePlayer(keys);
        bool endedTurn = EndTurn(keys);

        bool somethingHappened = changedView || changedPlayer || endedTurn;

        KeyHandlerOutput kho = new KeyHandlerOutput(somethingHappened, endedTurn);
        return kho;
    }
    bool ChangePlayer(Dictionary<KeyCode, KeyCodeInfo> keys)
    {
        KeyCode code = _changePlayers;
        bool changedPlayer = false;
        if (keys.ContainsKey(code))
        {
            KeyCodeInfo kci = keys[code];
            Dictionary<eInput, bool> inputs = kci._keyCodeInfo;
            if (inputs[eInput.Up])
            {
                _game._playerSystem.ChangePlayer();
                changedPlayer = true;
            }
        }
        return changedPlayer;
    }
    bool ChangeView(Dictionary<KeyCode, KeyCodeInfo> keys)
    {
        KeyCode code = _changeView;
        bool changedView = false;
        if (keys.ContainsKey(code))
        {
            KeyCodeInfo kci = keys[code];
            Dictionary<eInput, bool> inputs = kci._keyCodeInfo;
            if (inputs[eInput.Up])
            {
                _game._viewer.ToggleNextView();
                changedView = true;
            }
        }
        return changedView;
    }
    bool EndTurn(Dictionary<KeyCode, KeyCodeInfo> keys)
    {
        KeyCode code = _endTurn;
        bool endedTurn = false;
        if (keys.ContainsKey(code))
        {
            KeyCodeInfo kci = keys[code];
            Dictionary<eInput, bool> inputs = kci._keyCodeInfo;
            if (inputs[eInput.Up])
            {
                endedTurn = true;
            }
        }
        return endedTurn;
    }
}
