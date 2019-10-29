using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandlerOutput
{
    public bool _somethingHappened;
    public bool _turnEnded;
    public KeyHandlerOutput(bool happened, bool ended)
    {
        _somethingHappened = happened;
        _turnEnded = ended;
    }
}
