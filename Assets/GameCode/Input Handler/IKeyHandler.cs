using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IKeyHandler
{
    bool HandleKeys(KeyHandlerInfo keyHandlerInfo);
}
public class KeyHandlerHomelands : IKeyHandler
{
    public bool HandleKeys(KeyHandlerInfo keyHandlerInfo)
    {
        return false;
    }
}

