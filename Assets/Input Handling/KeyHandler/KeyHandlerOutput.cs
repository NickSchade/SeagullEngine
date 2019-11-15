using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandlerOutput
{
    public Dictionary<ePlayerAction, bool> _keyActions;
    public KeyHandlerOutput(Dictionary<ePlayerAction,bool> keyActions)
    {
        _keyActions = keyActions;
    }
}
