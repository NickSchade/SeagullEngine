using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KeyHandlerInfo
{
    public Dictionary<KeyCode, KeyCodeInfo> _keysInterfaced;
    public KeyHandlerInfo(Dictionary<KeyCode, KeyCodeInfo> keysInterfaced)
    {
        _keysInterfaced = keysInterfaced;
    }
}
