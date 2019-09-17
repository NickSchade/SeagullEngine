using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyCodeInfo
{
    public Dictionary<eInput, bool> _keyCodeInfo;
    public KeyCodeInfo(Dictionary<eInput, bool> keyCodeInfo)
    {
        _keyCodeInfo = keyCodeInfo;
    }
}
