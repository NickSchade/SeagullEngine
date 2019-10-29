using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IKeyHandler
{
    KeyHandlerOutput HandleKeys(KeyHandlerInfo keyHandlerInfo);
}

